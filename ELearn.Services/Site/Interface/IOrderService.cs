using ELearn.Data.ReturnMessages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Services.Site.Interface
{
    public interface IOrderService
    {
        bool IsUserInCourse(string userId, string courseId);
        Task<bool> IsUserInCourseAsync(string userId, string courseId);

        Response AddToOrder(string userId, string courseId);
        Task<Response> AddToOrderAsync(string userId, string courseId);

        Response RemoveFromOrder(string userId, string courseId);
        Task<Response> RemoveFromOrderAsync(string userId, string courseId);

        Response UpdateOrder(string userId);
        Task<Response> UpdateOrderAsync(string userId);
    }
}
