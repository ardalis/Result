using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Ardalis.Result.AspNetCore
{
    public static class MvcOptionsExtensions
    {
        public static MvcOptions AddDefaultResultConvention(this MvcOptions options)
        {
            var resultStatusMap = new ResultStatusMap();
            resultStatusMap.AddDefaultMap();

            options.Conventions.Add(new ResultConvention(resultStatusMap));

            return options;
        }

        public static MvcOptions AddResultConvention(this MvcOptions options, Action<ResultStatusMap> configure = null)
        {
            var resultStatusMap = new ResultStatusMap();
            configure?.Invoke(resultStatusMap);

            options.Conventions.Add(new ResultConvention(resultStatusMap));

            return options;
        }
    }
}

