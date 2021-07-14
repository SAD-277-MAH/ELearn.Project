using ELearn.Data.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ELearn.Common.Filters
{
    public class DocumentApproveFilter : ActionFilterAttribute
    {
        private readonly DatabaseContext _db;

        public DocumentApproveFilter(DatabaseContext db)
        {
            _db = db;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.RouteData.Values["userId"] != null)
            {
                var result = _db.Teachers.Where(t => t.UserId == context.RouteData.Values["userId"].ToString() && t.Status == 1);

                if (!result.Any())
                {
                    context.Result = new ForbidResult();
                }

                base.OnActionExecuting(context);
            }
            else if (context.RouteData.Values["id"] != null)
            {
                var result = _db.Teachers.Where(t => t.UserId == context.RouteData.Values["id"].ToString() && t.Status == 1);

                if (!result.Any())
                {
                    context.Result = new ForbidResult();
                }

                base.OnActionExecuting(context);
            }
        }
    }
}
