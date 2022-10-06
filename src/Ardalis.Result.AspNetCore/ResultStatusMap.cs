using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Ardalis.Result.AspNetCore
{
    /// <summary>
    /// A map of <see cref="ResultStatus"/>es to <see cref="HttpStatusCode"/>s combined with other configuration
    /// </summary>
    public class ResultStatusMap
    {
        private readonly Dictionary<ResultStatus, ResultStatusOptions> _map = new Dictionary<ResultStatus, ResultStatusOptions>();

        internal ResultStatusMap()
        {
        }

        public IEnumerable<ResultStatus> Keys => _map.Keys;

        /// <summary>
        /// Adds default mapping for all known <see cref="ResultStatus"/>es to <see cref="HttpStatusCode"/>s
        /// </summary>
        public ResultStatusMap AddDefaultMap()
        {
            return For(ResultStatus.Ok, HttpStatusCode.OK)
                .For(ResultStatus.Error, (HttpStatusCode)422, resultStatusOptions => resultStatusOptions
                    .With(UnprocessableEntity))
                .For(ResultStatus.Forbidden, HttpStatusCode.Forbidden)
                .For(ResultStatus.Unauthorized, HttpStatusCode.Unauthorized)
                .For(ResultStatus.Invalid, HttpStatusCode.BadRequest, resultStatusOptions => resultStatusOptions
                    .With(BadRequest))
                .For(ResultStatus.NotFound, HttpStatusCode.NotFound, resultStatusOptions => resultStatusOptions
                    .With(NotFoundEntity));
        }

        /// <summary>
        /// Maps <paramref name="status"/> to <paramref name="defaultStatusCode"/>.
        /// Allows to override default status code for specific Http Methods
        /// </summary>
        /// <param name="status">Result Status to map.</param>
        /// <param name="defaultStatusCode">Default Status Code.<param>
        /// <param name="configure">A <see cref="Action"/> to configure Status Codes for specific Http Methods.</param>
        public ResultStatusMap For(ResultStatus status, HttpStatusCode defaultStatusCode, Action<ResultStatusOptions> configure)
        {
            var info = new ResultStatusOptions(status, defaultStatusCode);
            configure(info);

            this[status] = info;
            return this;
        }

        /// <summary>
        /// Maps <paramref name="status"/> to <paramref name="statusCode"/>.
        /// </summary>
        /// <param name="status">Result Status to map.</param>
        /// <param name="statusCode">Status Code.</param>
        public ResultStatusMap For(ResultStatus status, HttpStatusCode statusCode)
        {
            this[status] = new ResultStatusOptions(status, statusCode);
            return this;
        }

        /// <summary>
        /// Remove mapping for <paramref name="status"/>
        /// </summary>
        /// <param name="status"></param>
        public ResultStatusMap Remove(ResultStatus status)
        {
            _map.Remove(status);
            return this;
        }

        /// <summary>
        /// Determines whether there is a mapping for given <paramref name="status"/>
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool ContainsKey(ResultStatus status)
        {
            return _map.ContainsKey(status);
        }

        internal ResultStatusOptions this[ResultStatus status]
        {
            get { return _map[status]; }
            set { _map[status] = value; }
        }

        private static ValidationProblemDetails BadRequest(ControllerBase controller, IResult result)
        {
            foreach (var error in result.ValidationErrors)
            {
                controller.ModelState.AddModelError(error.Identifier, error.ErrorMessage);
            }

            return new ValidationProblemDetails(controller.ModelState);
        }

        private static ProblemDetails UnprocessableEntity(ControllerBase controller, IResult result)
        {
            var details = new StringBuilder("Next error(s) occured:");

            foreach (var error in result.Errors) details.Append("* ").Append(error).AppendLine();

            return new ProblemDetails
            {
                Title = "Something went wrong.",
                Detail = result.Errors.Any() ? details.ToString() : null
            };
        }

        private static ProblemDetails NotFoundEntity(ControllerBase controller, IResult result)
        {
            var details = new StringBuilder("Next error(s) occured:");

            foreach (var error in result.Errors) details.Append("* ").Append(error).AppendLine();

            return new ProblemDetails
            {
                Title = "Resource not found.",
                Detail = result.Errors.Any() ? details.ToString() : null
            };
        }
    }

    public class ResultStatusOptions
    {
        private readonly Dictionary<string, HttpStatusCode> _methodToStatusMap = new Dictionary<string, HttpStatusCode>();
        private readonly HttpStatusCode _defaultStatusCode;

        internal ResultStatusOptions(ResultStatus status, HttpStatusCode defaultStatusCode)
        {
            _defaultStatusCode = defaultStatusCode;

            Status = status;
        }

        internal ResultStatusOptions(ResultStatus status, HttpStatusCode defaultStatusCode, Type responseType, Func<ControllerBase, IResult, object> getResponseObject)
        {
            _defaultStatusCode = defaultStatusCode;

            Status = status;
            ResponseType = responseType;
            GetResponseObject = getResponseObject;
        }

        internal ResultStatus Status { get; }
        internal Type ResponseType { get; private set; }
        internal Func<ControllerBase, IResult, object> GetResponseObject { get; private set; }

        /// <summary>
        /// Gets Http Status Code for specific Http Method.
        /// </summary>
        /// <param name="method"></param>
        /// <returns>Http Status Code for specific Http Method if configured, otherwise default Http Status Code.</returns>
        public HttpStatusCode GetStatusCode(string method)
        {
            method = method?.ToLower();

            if (string.IsNullOrEmpty(method) || !_methodToStatusMap.ContainsKey(method)) return _defaultStatusCode;

            return _methodToStatusMap[method];
        }

        /// <summary>
        /// Maps <paramref name="method"/> to a <paramref name="statusCode"/>
        /// </summary>
        /// <param name="method">Http Method.</param>
        /// <param name="statusCode">Http Status Code.</param>
        public ResultStatusOptions For(string method, HttpStatusCode statusCode)
        {
            _methodToStatusMap[method.ToLower()] = statusCode;
            return this;
        }

        /// <summary>
        /// Sets GetResponseObject callback.
        /// </summary>
        /// <param name="getResponseObject">A <see cref="Func"/> to extract response object from Result object for specific Result Status.</param>
        public ResultStatusOptions With<T>(Func<ControllerBase, IResult, T> getResponseObject)
        {
            ResponseType = typeof(T);
            GetResponseObject = (ctrlr, result) => getResponseObject(ctrlr, result);
            return this;
        }

        /// <summary>
        /// Sets GetResponseObject callback.
        /// This overload may be useful when response object type is converted to different type before serialization.
        /// For example, when returning <see cref="Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary"/>, <see cref="IDictionary{string, string[]}"/> is actually serialized.
        /// </summary>
        /// <param name="responseType">Response Type.</param>
        /// <param name="getResponseObject">A <see cref="Func"/> to extract response object from Result object for specific Result Status.</param>
        /// <returns></returns>
        public ResultStatusOptions With(Type responseType, Func<ControllerBase, IResult, object> getResponseObject)
        {
            ResponseType = responseType;
            GetResponseObject = getResponseObject;
            return this;
        }
    }
}
