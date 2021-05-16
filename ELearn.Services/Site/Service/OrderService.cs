using ELearn.Data.Context;
using ELearn.Data.Models;
using ELearn.Data.ReturnMessages;
using ELearn.Repo.Infrastructure;
using ELearn.Services.Site.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Services.Site.Service
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork<DatabaseContext> _db;

        public OrderService(IUnitOfWork<DatabaseContext> db)
        {
            _db = db;
        }

        public Response AddToOrder(string userId, string courseId)
        {
            var order = _db.OrderRepository.Get(o => o.UserId == userId && !o.Status, string.Empty);
            var course = _db.CourseRepository.Get(courseId);

            if (course == null)
            {
                return new Response(false, "دوره یافت نشد");
            }

            if (IsUserInCourse(userId, courseId))
            {
                return new Response(false, "کاربر این دوره را خریداری کرده است");
            }

            if (order == null)
            {
                order = new Order()
                {
                    OrderSum = course.Price,
                    Discount = 0,
                    Status = false,
                    UserId = userId
                };
                _db.OrderRepository.Add(order);
                _db.Save();
            }

            var orderDetail = _db.OrderDetailRepository.Get(o => o.OrderId == order.Id && o.CourseId == courseId, string.Empty);

            if (orderDetail != null)
            {
                return new Response(false, "دوره در سبد خرید وجود دارد");
            }

            orderDetail = new OrderDetail()
            {
                Price = course.Price,
                OrderId = order.Id,
                CourseId = courseId
            };
            _db.OrderDetailRepository.Add(orderDetail);
            _db.Save();

            var result = UpdateOrder(userId);
            result.Status = true;
            return result;
        }

        public async Task<Response> AddToOrderAsync(string userId, string courseId)
        {
            var order = await _db.OrderRepository.GetAsync(o => o.UserId == userId && !o.Status, string.Empty);
            var course = await _db.CourseRepository.GetAsync(courseId);

            if (course == null)
            {
                return new Response(false, "دوره یافت نشد");
            }

            if (await IsUserInCourseAsync(userId, courseId))
            {
                return new Response(false, "کاربر این دوره را خریداری کرده است");
            }

            if (order == null)
            {
                order = new Order()
                {
                    OrderSum = course.Price,
                    Discount = 0,
                    Status = false,
                    UserId = userId
                };
                await _db.OrderRepository.AddAsync(order);
                await _db.SaveAsync();
            }

            var orderDetail = await _db.OrderDetailRepository.GetAsync(o => o.OrderId == order.Id && o.CourseId == courseId, string.Empty);

            if (orderDetail != null)
            {
                return new Response(false, "دوره در سبد خرید وجود دارد");
            }

            orderDetail = new OrderDetail()
            {
                Price = course.Price,
                OrderId = order.Id,
                CourseId = courseId
            };
            await _db.OrderDetailRepository.AddAsync(orderDetail);
            await _db.SaveAsync();

            var result = await UpdateOrderAsync(userId);
            result.Status = true;
            return result;
        }

        public bool IsUserInCourse(string userId, string courseId)
        {
            var result = _db.UserCourseRepository.Get(u => u.UserId == userId && u.CourseId == courseId, string.Empty);
            return result != null;
        }

        public async Task<bool> IsUserInCourseAsync(string userId, string courseId)
        {
            var result = await _db.UserCourseRepository.GetAsync(u => u.UserId == userId && u.CourseId == courseId, string.Empty);
            return result != null;
        }

        public Response RemoveFromOrder(string userId, string courseId)
        {
            var order = _db.OrderRepository.Get(o => o.UserId == userId && !o.Status, string.Empty);
            var course = _db.CourseRepository.Get(courseId);

            if (course == null)
            {
                return new Response(false, "کالا یافت نشد");
            }

            if (order == null)
            {
                return new Response(false, "سبد خرید یافت نشد");
            }

            var orderDetail = _db.OrderDetailRepository.Get(o => o.OrderId == order.Id && o.CourseId == courseId, string.Empty);

            if (orderDetail == null)
            {
                return new Response(false, "کالا در سبد خرید یافت نشد");
            }

            _db.OrderDetailRepository.Delete(orderDetail);
            _db.Save();

            var result = UpdateOrder(userId);
            result.Status = true;
            return result;
        }

        public async Task<Response> RemoveFromOrderAsync(string userId, string courseId)
        {
            var order = await _db.OrderRepository.GetAsync(o => o.UserId == userId && !o.Status, string.Empty);
            var course = await _db.CourseRepository.GetAsync(courseId);

            if (course == null)
            {
                return new Response(false, "کالا یافت نشد");
            }

            if (order == null)
            {
                return new Response(false, "سبد خرید یافت نشد");
            }

            var orderDetail = await _db.OrderDetailRepository.GetAsync(o => o.OrderId == order.Id && o.CourseId == courseId, string.Empty);

            if (orderDetail == null)
            {
                return new Response(false, "کالا در سبد خرید یافت نشد");
            }

            _db.OrderDetailRepository.Delete(orderDetail);
            await _db.SaveAsync();

            var result = await UpdateOrderAsync(userId);
            result.Status = true;
            return result;
        }

        public Response UpdateOrder(string userId)
        {
            bool isChanged = false;
            List<string> messages = new List<string>();

            var order = _db.OrderRepository.Get(o => o.UserId == userId && !o.Status, string.Empty);

            if (order == null)
            {
                return new Response(false, "سبد خرید یافت نشد");
            }

            var orderDetails = _db.OrderDetailRepository.Get(o => o.OrderId == order.Id, null, string.Empty);
            int orderSum = 0;
            foreach (var orderDetail in orderDetails)
            {
                var course = _db.CourseRepository.Get(orderDetail.CourseId);

                if (course == null)
                {
                    messages.Add($"دوره {orderDetail.CourseId} یافت نشد");
                    _db.OrderDetailRepository.Delete(orderDetail.Id);
                    _db.Save();
                    isChanged = true;
                }

                if (IsUserInCourse(userId, orderDetail.CourseId))
                {
                    messages.Add($"کاربر دوره {course.Title} را خریداری کرده است");
                    _db.OrderDetailRepository.Delete(orderDetail.Id);
                    _db.Save();
                    isChanged = true;
                }

                if (course.Price != orderDetail.Price)
                {
                    messages.Add($"قیمت دوره {course.Title} تغییر کرده است");
                    orderDetail.Price = course.Price;
                    _db.OrderDetailRepository.Update(orderDetail);
                    _db.Save();
                    isChanged = true;
                }

                orderSum += orderDetail.Price;
            }
            if (order.OrderSum != orderSum)
            {
                messages.Add($"مجموع سبد خرید تغییر کرده است");
                order.OrderSum = orderSum;
                _db.OrderRepository.Update(order);
                _db.Save();
                isChanged = true;
            }

            return new Response(isChanged, messages);
        }

        public async Task<Response> UpdateOrderAsync(string userId)
        {
            bool isChanged = false;
            List<string> messages = new List<string>();

            var order = await _db.OrderRepository.GetAsync(o => o.UserId == userId && !o.Status, string.Empty);

            if (order == null)
            {
                return new Response(false, "سبد خرید یافت نشد");
            }

            var orderDetails = await _db.OrderDetailRepository.GetAsync(o => o.OrderId == order.Id, null, string.Empty);
            int orderSum = 0;
            foreach (var orderDetail in orderDetails)
            {
                var course = await _db.CourseRepository.GetAsync(orderDetail.CourseId);

                if (course == null)
                {
                    messages.Add($"دوره {orderDetail.CourseId} یافت نشد");
                    _db.OrderDetailRepository.Delete(orderDetail.Id);
                    await _db.SaveAsync();
                    isChanged = true;
                }

                if (await IsUserInCourseAsync(userId, orderDetail.CourseId))
                {
                    messages.Add($"کاربر دوره {course.Title} را خریداری کرده است");
                    _db.OrderDetailRepository.Delete(orderDetail.Id);
                    await _db.SaveAsync();
                    isChanged = true;
                }

                if (course.Price != orderDetail.Price)
                {
                    messages.Add($"قیمت دوره {course.Title} تغییر کرده است");
                    orderDetail.Price = course.Price;
                    _db.OrderDetailRepository.Update(orderDetail);
                    await _db.SaveAsync();
                    isChanged = true;
                }

                orderSum += orderDetail.Price;
            }
            if (order.OrderSum != orderSum)
            {
                messages.Add($"مجموع سبد خرید تغییر کرده است");
                order.OrderSum = orderSum;
                _db.OrderRepository.Update(order);
                await _db.SaveAsync();
                isChanged = true;
            }

            return new Response(isChanged, messages);
        }
    }
}
