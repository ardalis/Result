using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Ardalis.Result.AspNetCore
{
    public static class MvcOptionsExtensions
    {
        /// <summary>
        /// Adds <see cref="ResultConvention"/> which generates <see cref="ProducesResponseTypeAttribute"/>s
        /// for every endpoint marked with <see cref="TranslateResultToActionResultAttribute"/>
        /// based on default configuration
        /// </summary>
        public static MvcOptions AddDefaultResultConvention(this MvcOptions options)
        {
            var resultStatusMap = new ResultStatusMap();
            resultStatusMap.AddDefaultMap();

            options.Conventions.Add(new ResultConvention(resultStatusMap));

            return options;
        }

        /// <summary>
        /// Adds <see cref="ResultConvention"/> which generates <see cref="ProducesResponseTypeAttribute"/>s
        /// for every endpoint marked with <see cref="TranslateResultToActionResultAttribute"/>
        /// based on provided configuration
        /// </summary>
        /// <param name="configure">A <see cref="Action"/> to map <see cref="ResultStatus"/>es to <see cref="System.Net.HttpStatusCode"/>s</param>
        public static MvcOptions AddResultConvention(this MvcOptions options, Action<ResultStatusMap> configure = null)
        {
            var resultStatusMap = new ResultStatusMap();
            configure?.Invoke(resultStatusMap);

            options.Conventions.Add(new ResultConvention(resultStatusMap));

            return options;
        }
    }
}

