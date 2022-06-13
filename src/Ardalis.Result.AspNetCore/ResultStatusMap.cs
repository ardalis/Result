using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Ardalis.Result.AspNetCore
{
    public class ResultStatusMap
    {
        private readonly Dictionary<ResultStatus, ResultStatusOptions> _map = new Dictionary<ResultStatus, ResultStatusOptions>();

        internal ResultStatusMap()
        {
        }

        public IEnumerable<ResultStatus> Keys => _map.Keys;

        public ResultStatusMap AddDefaultMap()
        {
            return For(ResultStatus.Ok, HttpStatusCode.OK)
                .For(ResultStatus.Error, (HttpStatusCode)422, (_, result) => {
                    var details = new StringBuilder("Next error(s) occured:");

                    foreach (var error in result.Errors) details.Append("* ").Append(error).AppendLine();

                    return new ProblemDetails
                    {
                        Title = "Something went wrong.",
                        Detail = details.ToString()
                    };
                })
                .For(ResultStatus.Forbidden, HttpStatusCode.Forbidden)
                .For(ResultStatus.Unauthorized, HttpStatusCode.Unauthorized)
                .For(ResultStatus.Invalid, HttpStatusCode.BadRequest, typeof(IDictionary<string, string[]>), (ctrlr, result) => {
                    foreach (var error in result.ValidationErrors)
                    {
                        ctrlr.ModelState.AddModelError(error.Identifier, error.ErrorMessage);
                    }

                    return ctrlr.ModelState;
                })
                .For(ResultStatus.NotFound, HttpStatusCode.NotFound);
        }

        public ResultStatusMap For(ResultStatus status, HttpStatusCode defaultStatusCode, Action<ResultStatusOptions> configure)
        {
            var info = new ResultStatusOptions(status, defaultStatusCode);
            configure(info);

            this[status] = info;
            return this;
        }

        public ResultStatusMap For(ResultStatus status, HttpStatusCode statusCode)
        {
            this[status] = new ResultStatusOptions(status, statusCode);
            return this;
        }

        public ResultStatusMap For<T>(ResultStatus status, HttpStatusCode statusCode, Func<ControllerBase, IResult, T> getResponseObject)
        {
            this[status] = new ResultStatusOptions(status, statusCode, typeof(T), (ctrlr, result) => getResponseObject(ctrlr, result));
            return this;
        }

        public ResultStatusMap For(ResultStatus status, HttpStatusCode statusCode, Type responseType, Func<ControllerBase, IResult, object> getResponseObject)
        {
            this[status] = new ResultStatusOptions(status, statusCode, responseType, getResponseObject);
            return this;
        }

        public ResultStatusMap Remove(ResultStatus status)
        {
            _map.Remove(status);
            return this;
        }

        public bool ContainsKey(ResultStatus status)
        {
            return _map.ContainsKey(status);
        }

        public ResultStatusOptions this[ResultStatus status]
        {
            get { return _map[status]; }
            set { _map[status] = value; }
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

        public ResultStatus Status { get; }
        public Type ResponseType { get; private set; }
        public Func<ControllerBase, IResult, object> GetResponseObject { get; }

        public HttpStatusCode GetStatusCode(string method)
        {
            method = method?.ToLower();

            if (string.IsNullOrEmpty(method) || !_methodToStatusMap.ContainsKey(method)) return _defaultStatusCode;

            return _methodToStatusMap[method];
        }

        public ResultStatusOptions Override(string method, HttpStatusCode statusCode)
        {
            _methodToStatusMap[method.ToLower()] = statusCode;
            return this;
        }
    }
}
