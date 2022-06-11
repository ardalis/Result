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

namespace Ardalis.Result.AspNetCore
{
    public class ResultConvention : IActionModelConvention
    {
        public const string RESULT_STATUS_MAP_PROP = "ResultStatusMap";

        private readonly ResultStatusMap _map;

        public ResultConvention(ResultStatusMap map)
        {
            _map = map;
        }

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

                var successStatusCode = !string.IsNullOrEmpty(method) && _map.ContainsKey(ResultStatus.Ok)
                    ? _map[ResultStatus.Ok].GetStatusCode(method)
                    : HttpStatusCode.OK;
                var successType = isVoidResult ? typeof(void) : returnType.GetGenericArguments()[0];

                AddProducesResponseTypeAttribute(action.Filters, (int)successStatusCode, successType);

                var attr = action.Filters.SingleOrDefault(f => f is ExpectedFailureResultStatusesAttribute) as ExpectedFailureResultStatusesAttribute;
                var resultStatuses = attr?.ResultStatuses ?? _map.Keys;

                foreach (var status in resultStatuses.Where(s => _map.ContainsKey(s)))
                {
                    var info = _map[status];
                    AddProducesResponseTypeAttribute(action.Filters, (int)info.GetStatusCode(method), info.ResponseType);
                }
            }
        }

        private void AddProducesResponseTypeAttribute(IList<IFilterMetadata> filters, int statusCode, Type responseType)
        {
            if (!filters.Any(f => f is IApiResponseMetadataProvider rmp && rmp.StatusCode == statusCode))
            {
                filters.Add(responseType == null ? new ProducesResponseTypeAttribute(statusCode) : new ProducesResponseTypeAttribute(responseType, statusCode));
            }
        }
    }
}
