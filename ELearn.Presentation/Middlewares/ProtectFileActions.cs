using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELearn.Presentation.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ProtectFileActions
    {
        private readonly RequestDelegate _next;

        public ProtectFileActions(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var path = httpContext.Request.Path.ToString().ToLower();
            if (path.StartsWith("/streamfile") || path.StartsWith("/downloadfile"))
            {
                var token = httpContext.Request.Query["token"].ToString();

                httpContext.Request.Headers.Add("Authorization", $"Bearer {token}");

                var qb = new QueryBuilder();
                httpContext.Request.QueryString = qb.ToQueryString();
            }
            
            await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ProtectFileActionsExtensions
    {
        public static IApplicationBuilder UseProtectFileActions(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ProtectFileActions>();
        }
    }
}
