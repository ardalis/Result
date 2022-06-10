using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Ardalis.Result.AspNetCore
{
    public class ResultStatusMap : Dictionary<ResultStatus, ResultStatusOptions>
    {
        public ResultStatusMap AddDefaultMap()
        {
            return For(ResultStatus.Ok, HttpStatusCode.OK)
                .For(ResultStatus.Error, (HttpStatusCode)422, result => {
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
                .For(ResultStatus.Invalid, HttpStatusCode.BadRequest, typeof(IEnumerable<string>), result => {
                    var errors = new List<string>();

                    foreach (var error in result.ValidationErrors) errors.Add(error.ErrorMessage);

                    return errors;
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

        public ResultStatusMap For<T>(ResultStatus status, HttpStatusCode statusCode, Func<IResult, T> getResponseObject)
        {
            this[status] = new ResultStatusOptions(status, statusCode, typeof(T), result => getResponseObject(result));
            return this;
        }

        public ResultStatusMap For(ResultStatus status, HttpStatusCode statusCode, Type responseType, Func<IResult, object> getResponseObject)
        {
            this[status] = new ResultStatusOptions(status, statusCode, responseType, getResponseObject);
            return this;
        }

        public new ResultStatusMap Remove(ResultStatus status)
        {
            base.Remove(status);
            return this;
        }
    }

    public class ResultStatusOptions
    {
        private Dictionary<string, HttpStatusCode> _methodToStatusMap = new Dictionary<string, HttpStatusCode>();
        private HttpStatusCode _defaultStatusCode;

        public ResultStatusOptions(ResultStatus status, HttpStatusCode defaultStatusCode)
        {
            _defaultStatusCode = defaultStatusCode;

            Status = status;
        }

        public ResultStatusOptions(ResultStatus status, HttpStatusCode defaultStatusCode, Type responseType, Func<IResult, object> getResponseObject)
        {
            _defaultStatusCode = defaultStatusCode;

            Status = status;
            ResponseType = responseType;
            GetResponseObject = getResponseObject;
        }

        public ResultStatus Status { get; }
        public Type ResponseType { get; private set; }
        public Func<IResult, object> GetResponseObject { get; }

        public HttpStatusCode GetStatusCode(string method)
        {
            method = method.ToLower();

            if (!_methodToStatusMap.ContainsKey(method)) return _defaultStatusCode;

            return _methodToStatusMap[method];
        }

        public ResultStatusOptions Override(string method, HttpStatusCode statusCode)
        {
            _methodToStatusMap[method.ToLower()] = statusCode;
            return this;
        }
    }
}
