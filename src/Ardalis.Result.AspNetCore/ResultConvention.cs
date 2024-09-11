using Ardalis.Result.AspNetCore.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Ardalis.Result.AspNetCore.UnitTests")]

namespace Ardalis.Result.AspNetCore
{
    internal class ResultConvention : IActionModelConvention
    {
        public const string RESULT_STATUS_MAP_PROP = "ResultStatusMap";

        private readonly ResultStatusMap _map;

        internal ResultConvention(ResultStatusMap map) => _map = map;

        public void Apply(ActionModel action)
        {
            if (!action.Filters.Any(f => f is TranslateResultToActionResultAttribute tr)
                && !action.Controller.Filters.Any(f => f is TranslateResultToActionResultAttribute tr)) return;

            action.Properties[RESULT_STATUS_MAP_PROP] = _map;

            var returnType = action.ActionMethod.ReturnType;

            if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
            {
                returnType = returnType.GetGenericArguments()[0];
            }

            var isResult = returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Result<>);
            var isVoidResult = (isResult && returnType.GetGenericArguments()[0] == typeof(Result))
                || returnType.IsSubclassOf(typeof(Result<Result>));

            if (isResult || isVoidResult)
            {
                var method = (action.Attributes.FirstOrDefault(a => a is HttpMethodAttribute) as HttpMethodAttribute)?.HttpMethods.FirstOrDefault();

                var successStatusCode = _map.ContainsKey(ResultStatus.Ok)
                    ? _map[ResultStatus.Ok].GetStatusCode(method)
                    : HttpStatusCode.OK;

                /*
                    Following If statement is added to play along with Api Explorer behavior:
                    https://github.com/dotnet/aspnetcore/blob/e5f183b656a0e8bc087108130a5a9b54ae94494e/src/Mvc/Mvc.ApiExplorer/src/ApiResponseTypeProvider.cs#L159

                    In simple terms, if controller action return type is anything but void (suppose in our case it returns 'void' Result), 
                    and [ProducesResponseType] attribute has response type System.Void (even if deliberately set by developer),
                    it will be overwriten by Api Explorer to match action return type if Status Code is 200 or 201.

                    So we should avoid pairing 'void' Result with OK or Created
                */
                if (isVoidResult && (successStatusCode == HttpStatusCode.OK || successStatusCode == HttpStatusCode.Created))
                    successStatusCode = HttpStatusCode.NoContent;

                var successType = isVoidResult ? null : returnType.GetGenericArguments()[0];

                AddProducesResponseTypeAttribute(action.Filters, (int)successStatusCode, successType);

                var attr = action.Attributes.SingleOrDefault(f => f is ExpectedFailuresAttribute) as ExpectedFailuresAttribute;

                if (attr?.ResultStatuses != null)
                {
                    var unexpectedResults = attr.ResultStatuses.Where(s => !_map.Keys.Contains(s));
                    if (unexpectedResults.Any())
                    {
                        throw new UnexpectedFailureResultsException(unexpectedResults);
                    }
                }

                var resultStatuses = attr?.ResultStatuses ?? _map.Keys;

                foreach (var status in resultStatuses.Where(s => 
                             s is not (ResultStatus.Ok or ResultStatus.Created)))
                {
                    var info = _map[status];
                    AddProducesResponseTypeAttribute(action.Filters, (int)info.GetStatusCode(method), info.ResponseType);
                }
            }
        }

        private static void AddProducesResponseTypeAttribute(IList<IFilterMetadata> filters, int statusCode, Type responseType)
        {
            if (!filters.Any(f => f is IApiResponseMetadataProvider rmp && rmp.StatusCode == statusCode))
            {
                filters.Add(responseType == null ? new ProducesResponseTypeAttribute(statusCode) : new ProducesResponseTypeAttribute(responseType, statusCode));
            }
        }
    }
}
