﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;

namespace Ardalis.Result.AspNetCore.UnitTests;
public class BaseResultConventionTest
{
    protected bool ProducesResponseTypeAttribute(IFilterMetadata filterMetadata, int statusCode, Type type)
    {
        return filterMetadata is ProducesResponseTypeAttribute attr
            && attr.StatusCode == statusCode && attr.Type == type;
    }

    public class TestController : ControllerBase
    {
        public Result Index()
        {
            throw new NotImplementedException();
        }

        public Result<string> ResultString()
        {
            throw new NotImplementedException();
        }

        public Result<IEnumerable<string>> ResultEnumerableString()
        {
            throw new NotImplementedException();
        }
    }

    public class ActionModelBuilder
    {
        private List<object> _actionAttributes = new List<object>();
        private List<IFilterMetadata> _actionFilters = new List<IFilterMetadata>();
        private List<IFilterMetadata> _actionControllerFilters = new List<IFilterMetadata>();

        public ActionModelBuilder AddActionFilter(IFilterMetadata filterMetadata)
        {
            _actionFilters.Add(filterMetadata);
            return this;
        }

        public ActionModelBuilder AddControllerFilter(IFilterMetadata filterMetadata)
        {
            _actionControllerFilters.Add(filterMetadata);
            return this;
        }

        public ActionModelBuilder AddActionAttribute(Attribute attr)
        {
            _actionAttributes.Add(attr);
            return this;
        }

        public ActionModel GetActionModel(string actionName = "Index")
        {
            var actionModel = new ActionModel(typeof(TestController).GetMethod(actionName), _actionAttributes);
            actionModel.Controller = new ControllerModel(typeof(TestController).GetTypeInfo(), new List<object>().AsReadOnly());

            foreach (var filter in _actionFilters)
            {
                actionModel.Filters.Add(filter);
            }

            foreach(var filter in _actionControllerFilters)
            {
                actionModel.Controller.Filters.Add(filter);
            }

            return actionModel;
        }
    }
}
