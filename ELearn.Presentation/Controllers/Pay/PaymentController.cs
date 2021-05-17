using ELearn.Data.Context;
using ELearn.Data.Models;
using ELearn.Data.ViewModels;
using ELearn.Repo.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZarinpalSandbox;

namespace ELearn.Presentation.Controllers.Pay
{
    public class PaymentController : Controller
    {
        private readonly IUnitOfWork<DatabaseContext> _db;

        public PaymentController(IUnitOfWork<DatabaseContext> db)
        {
            _db = db;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Verify(string Id)
        {
            var order = await _db.OrderRepository.GetAsync(o => o.Id == Id, "OrderDetails");
            if (order == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(HttpContext.Request.Query["Status"]) &&
                HttpContext.Request.Query["Status"].ToString().ToLower() == "ok" &&
                !string.IsNullOrEmpty(HttpContext.Request.Query["Authority"]))
            {
                string authority = HttpContext.Request.Query["Authority"].ToString();
                var payment = new Payment(Convert.ToInt32(order.OrderSum - order.Discount));
                var res = payment.Verification(authority).Result;

                if (res.Status == 100)
                {
                    order.Status = true;
                    _db.OrderRepository.Update(order);
                    foreach (var item in order.OrderDetails)
                    {
                        var userCourse = new UserCourse()
                        {
                            CourseId = item.CourseId,
                            UserId = order.UserId
                        };
                        await _db.UserCourseRepository.AddAsync(userCourse);
                    }

                    await _db.SaveAsync();

                    var result = new OrderFactorViewModel(true, order.OrderSum - order.Discount, "پرداخت با موفقیت انجام شد");
                    return View(result);
                }
            }
            else if (!string.IsNullOrEmpty(HttpContext.Request.Query["Status"]) &&
                HttpContext.Request.Query["Status"].ToString().ToLower() == "nok" &&
                !string.IsNullOrEmpty(HttpContext.Request.Query["Authority"]))
            {
                var result = new OrderFactorViewModel(false, order.OrderSum - order.Discount, "عملیات پرداخت ناموفق بود");
                return View(result);
            }

            return NotFound();
        }
    }
}
