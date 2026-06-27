# AGENTS.md

Guidance for AI coding agents (and human contributors) working in this repository.

## What This Is

`Ardalis.Result` implements the Result pattern: a way for service/domain methods to return success *or* one of many non-success states (NotFound, Invalid, Conflict, Forbidden, etc.) without throwing exceptions, in a form that maps cleanly onto HTTP status codes. The repo ships four independently versioned NuGet packages:

- **`Ardalis.Result`** (`src/Ardalis.Result`) — the core abstraction. No third-party dependencies. The other packages and any consumer depend on this.
- **`Ardalis.Result.AspNetCore`** (`src/Ardalis.Result.AspNetCore`) — translates `Result`/`Result<T>` into MVC `ActionResult` and Minimal API `IResult`. Depends on the ASP.NET Core shared framework.
- **`Ardalis.Result.FluentValidation`** (`src/Ardalis.Result.FluentValidation`) — a single extension (`ValidationResult.AsErrors()`) bridging FluentValidation failures into `ValidationError`s.
- **`Ardalis.Result.FluentAssertions`** (`src/Ardalis.Result.FluentAssertions`) — `ShouldBe*` test assertions for results.

Core/AspNetCore/FluentValidation are versioned together (currently `10.1.0`, set in each `.csproj`); FluentAssertions is on its own track (`1.0.0`). Release notes live inline in the `<PackageReleaseNotes>` of each `.csproj`.

## Common Commands

Run from the repo root.

```bash
dotnet restore
dotnet build --configuration Release --no-restore
dotnet test                                   # all test projects, all target frameworks
```

> **Local runtime gotcha:** the projects target `net6.0`/`net8.0` (and `net48`), but this machine only has the **.NET 10** SDK/runtime installed. Tests do **not** auto-roll-forward — `dotnet test` aborts with *"You must install or update .NET to run this application."* Either install the .NET 6 and .NET 8 runtimes (CI does this) or set `DOTNET_ROLL_FORWARD=LatestMajor` to run the existing targets on .NET 10:
>
> ```bash
> DOTNET_ROLL_FORWARD=LatestMajor dotnet test --framework net8.0
> ```

Run one test project:

```bash
dotnet test tests/Ardalis.Result.UnitTests/Ardalis.Result.UnitTests.csproj
```

Run a single test or class with a filter (xUnit; filter on the fully-qualified name or method):

```bash
dotnet test tests/Ardalis.Result.UnitTests/Ardalis.Result.UnitTests.csproj --filter "FullyQualifiedName~ResultBind"
```

Pin to one framework when a project multi-targets (avoids running the same test under net6.0/net8.0/net48):

```bash
dotnet test tests/Ardalis.Result.UnitTests/Ardalis.Result.UnitTests.csproj --framework net8.0
```

Run the sample apps:

```bash
dotnet run --project sample/Ardalis.Result.SampleWeb            # MVC + ApiEndpoints + MediatR sample, has Swagger
dotnet run --project sample/Ardalis.Result.SampleMinimalApi     # Minimal API sample (net8.0 only)
```

## Multi-targeting and Toolchain (read before touching `.csproj`/`Directory.Build.props`)

Target frameworks are **not** set in individual project files. They come from per-area `Directory.Build.props`, each of which defines `NetCoreFrameworks` (currently `net6.0;net8.0`) and a `TargetFrameworks` built from it:

- `src/Directory.Build.props` → `netstandard2.0;net6.0;net8.0`, plus packaging metadata (`GeneratePackageOnBuild`, authors, SourceLink, etc.).
- `tests/Directory.Build.props` → `net48;net6.0;net8.0`.
- `sample/Directory.Build.props` → `net48;net6.0;net8.0`, `Nullable` enabled.

To change the supported .NET versions across the whole repo, edit `NetCoreFrameworks` in these three files — not the individual projects. Projects that must pin a framework override `TargetFrameworks` locally (e.g. the Minimal API sample and its tests are `net8.0` only; `Ardalis.Result.AspNetCore` uses `$(NetCoreFrameworks)` so it never targets `net48`/`netstandard2.0`).

Other cross-cutting build facts:

- **Central Package Management** is on (`Directory.Packages.props`, `ManagePackageVersionsCentrally=true`). Add/upgrade dependency versions there with `<PackageVersion>`; reference them in `.csproj` with a bare `<PackageReference Include="..." />` (no `Version`). Use `VersionOverride` only for per-framework pins (the samples do this for the net6.0 ASP.NET Core packages).
- **PolySharp** is a global package reference, so modern C# syntax (`init` accessors, `record`, collection expressions like `[]`, `required`) compiles even on `netstandard2.0` and `net48`. Don't add manual polyfills for these.
- **SourceLink** (`Microsoft.SourceLink.GitHub`) is added only for packable projects on `net8.0+`.
- **net48 caveat:** `net48` is kept in tests/samples purely to verify the core library stays usable from .NET Framework. On non-Windows (CI and macOS), running `net48` tests requires **Mono** (`mono-devel`). `dotnet test` without Mono will skip/fail those targets — pin `--framework net8.0` (or net6.0) locally to avoid them. The active branch history shows net48 support is sensitive; verify net48 still builds when changing targets.

## CI and Publishing

- **`.github/workflows/dotnetcore.yml`** is the PR/main build: installs .NET 6 + 8 and Mono, then `restore` → `build -c Release` → `dotnet test` with `XPlat Code Coverage`. It has a **dedicated step that runs the three test projects explicitly against `--framework net48`** — if you touch multi-targeting, expect this step to catch regressions. Coverage is summarized and posted back to the PR by `comment-on-pr.yml`.
- **`publish-result.yml`** and **`publish-result-related.yml`** pack and push to NuGet on every push to `main` (with `--skip-duplicate`, so the version only publishes when bumped in the `.csproj`).
- **`jekyll-gh-pages.yml`** publishes `docs/` to <https://result.ardalis.com> on changes under `docs/`.

## Architecture

### Core result types (`src/Ardalis.Result`)

- **`ResultStatus`** (enum) is the spine of the whole library: `Ok, Created, Error, Forbidden, Unauthorized, Invalid, NotFound, NoContent, Conflict, CriticalError, Unavailable`. Almost every behavior is a `switch` over this enum, so **adding a new status means updating every such switch** — the translation maps in `ResultStatusMap`/`MinimalApiResultExtensions` and the `HandleNonSuccessStatus` helpers in `ResultExtensions` will all need a new arm (some throw `NotSupportedException` on unknown statuses).
- **`Result<T>`** is the central type. Instances are created through static factory methods (`Result<T>.Success(value)`, `.NotFound()`, `.Invalid(...)`, `.Created(...)`, `.Conflict(...)`, etc.), **not** the constructors. `IsSuccess` is true for `Ok`, `Created`, and `NoContent`. Carries `Value`, `Errors` (strings), `ValidationErrors`, `SuccessMessage`, `CorrelationId`, and `Location` (for `Created`).
- **`Result`** (non-generic, `Result.Void.cs`) inherits from `Result<Result>` — a deliberate quirk so the void result flows through the same generic machinery. It **re-declares** the factory methods with `new` (e.g. `Result.NotFound()` returns `Result`, while `Result<T>.NotFound()` returns `Result<T>`). When adding a factory/status, add it in *both* `Result.cs` and `Result.Void.cs`.
- **`IResult`** is the non-generic, value-type-erased contract (`Status`, `Errors`, `ValidationErrors`, `ValueType`, `GetValue()`, `Location`). The ASP.NET Core layer translates against `IResult` so it works uniformly for `Result` and `Result<T>`.
- **Implicit conversions** make the happy path terse: `T → Result<T>` (auto-success), `Result<T> → T` (unwrap `Value`), and `Result → Result<T>` (carry a non-success `Result` into a typed context). Be aware these exist when reasoning about overload resolution.
- **`Map`/`Bind` (`ResultExtensions.cs`)** implement Railway-Oriented Programming. `Map(func)` transforms `Value` on success and passes non-success states through unchanged; `Bind(func)` chains another `Result`-returning call and short-circuits on failure. Both have full sync + `Task`-based async overloads (`MapAsync`/`BindAsync`) covering `Result`/`Result<T>` in every position. All non-success propagation funnels through the private `HandleNonSuccessStatus` helpers, which rebuild the equivalent failed result of the destination type — keep these in sync with each other and with `ResultStatus`.
- **Supporting types:** `ValidationError` (`Identifier`, `ErrorMessage`, `ErrorCode`, `Severity`) + `ValidationSeverity` (`Error`/`Warning`/`Info`); `ErrorList` (record bundling error messages + optional `CorrelationId`); `PagedResult<T> : Result<T>` adds `PagedInfo` (use `result.ToPagedResult(pagedInfo)`); `IResultExtensions` adds `IsOk()`/`IsNotFound()`/etc. predicates over `IResult`.

### ASP.NET Core translation (`src/Ardalis.Result.AspNetCore`)

Two independent translation paths, both keyed off `ResultStatus`:

1. **MVC → `ActionResult`.** Either apply `[TranslateResultToActionResult]` (an `ActionFilterAttribute` that rewrites the action's `ObjectResult` in `OnActionExecuted`) or call `result.ToActionResult(this)` / `this.ToActionResult(result)` inside an action. Both funnel into the internal `ToActionResult(this ControllerBase, IResult)` in `ActionResultExtensions.cs`.
2. **Minimal API → `Microsoft.AspNetCore.Http.IResult`.** Call `result.ToMinimalApiResult()`. Implemented in `MinimalApiResultExtensions.cs`, guarded by `#if NET6_0_OR_GREATER`, mapping each status to `Results.Ok/NotFound/Conflict/Problem/...` with `ProblemDetails` payloads.

The status→HTTP mapping for the MVC path is configurable through **`ResultStatusMap`** / `ResultStatusOptions`. `AddDefaultMap()` defines the baseline (e.g. `Error → 422`, `Invalid → 400 ValidationProblemDetails`, `NotFound → 404 ProblemDetails`). Consumers customize it via `MvcOptionsExtensions`: `services.AddControllers(o => o.AddDefaultResultConvention())` or `o.AddResultConvention(map => map.For(...).Remove(...))`. The same map also drives **API metadata**: `ResultConvention` emits `[ProducesResponseType]` for each mapped status on every `[TranslateResultToActionResult]` endpoint (so Swagger/NSwag see them), and `[ExpectedFailures(...)]` narrows which statuses a given endpoint advertises. A status listed in `ExpectedFailures` must exist in the configured `ResultStatusMap`.

### Samples (`sample/`)

`Ardalis.Result.Sample.Core` holds framework-agnostic domain services/validators reused by the web samples and their tests. `SampleWeb` demonstrates the MVC/ApiEndpoints/MediatR usage (including a `BadApproaches/` folder showing the anti-patterns the library replaces); `SampleMinimalApi` demonstrates `ToMinimalApiResult`. The `*.FunctionalTests` projects spin these up via `Microsoft.AspNetCore.Mvc.Testing`.

## Testing Conventions

- **xUnit** throughout, with **FluentAssertions** for assertions, **Moq** where mocking is needed, and **coverlet** for coverage.
- Test files/classes are named `<TypeUnderTest><Member>` — one file per method/behavior under test (e.g. `ResultConstructor.cs`, `ResultBind.cs`, `ResultMapAsync.cs`, `PersonServiceCreate.cs`). Follow this pattern when adding tests rather than one large file per class.
- `Ardalis.Result.FluentAssertions.UnitTests` tests the assertion helpers themselves; its `FailureResults/` and `SuccessFullResults/` folders mirror each `ResultStatus`.
- Public API and behavior changes that span frameworks should be validated against `net48` as well (see the CI net48 step), since that target is the reason several compatibility constraints exist.
