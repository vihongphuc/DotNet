using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using System;

namespace GetSPODataListCallBackURL.Cores.Filter
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class QueryStringConstraintAttribute : ActionMethodSelectorAttribute
    {
        public string ValueName { get; set; }
        public bool ValuePresent { get; set; }

        public QueryStringConstraintAttribute(string valueName, bool valuePresent)
        {
            this.ValueName = valueName;
            this.ValuePresent = valuePresent;
        }
        public override bool IsValidForRequest(RouteContext routeContext, ActionDescriptor action)
        {
            var value = routeContext.HttpContext.Request.Query[this.ValueName];
            if (this.ValuePresent)
            {
                return !StringValues.IsNullOrEmpty(value);
            }

            return StringValues.IsNullOrEmpty(value);
        }
    }
}
