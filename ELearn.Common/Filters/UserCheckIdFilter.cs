using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace ELearn.Common.Filters
{
    public class UserCheckIdFilter : ActionFilterAttribute
    {
        private readonly ILogger _logger;

        public UserCheckIdFilter(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("UserCheckIdFilter");
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.RouteData.Values["userId"] != null)
            {
                if (context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value != context.RouteData.Values["userId"].ToString())
                {
                    context.Result = new UnauthorizedResult();
                }

                base.OnActionExecuting(context);
            }
            else if (context.RouteData.Values["id"] != null)
            {
                if (context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value != context.RouteData.Values["id"].ToString())
                {
                    context.Result = new UnauthorizedResult();
                }

                base.OnActionExecuting(context);
            }
        }
    }
}
