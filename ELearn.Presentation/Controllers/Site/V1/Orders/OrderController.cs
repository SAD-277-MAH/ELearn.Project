using AutoMapper;
using ELearn.Common.Filters;
using ELearn.Data.Context;
using ELearn.Data.Dtos.Site.Order;
using ELearn.Presentation.Routes.V1;
using ELearn.Repo.Infrastructure;
using ELearn.Services.Site.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELearn.Presentation.Controllers.Site.V1.Orders
{
    [ApiExplorerSettings(GroupName = "v1_Site")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IUnitOfWork<DatabaseContext> _db;
        private readonly IMapper _mapper;
        private readonly IOrderService _orderService;

        public OrderController(IUnitOfWork<DatabaseContext> db, IMapper mapper, IOrderService orderService)
        {
            _db = db;
            _mapper = mapper;
            _orderService = orderService;
        }

        [Authorize(Policy = "RequireStudentRole")]
        [HttpPost(ApiV1Routes.Order.GetOrder)]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        public async Task<IActionResult> GetOrder(string userId)
        {
            var result = await _orderService.UpdateOrderAsync(userId);

            var order = await _db.OrderRepository.GetAsync(o => o.UserId == userId && !o.Status, "OrderDetails");
            if (order == null)
            {
                return Ok();
            }

            var basket = _mapper.Map<OrderForDetailedDto>(order);
            foreach (var item in basket.OrderDetails)
            {
                var course = await _db.CourseRepository.GetAsync(item.CourseId);
                item.Title = course.Title;
                item.PhotoUrl = course.PhotoUrl;
            }

            return Ok(new OrderForReturnDto(basket, result));
        }

        [Authorize(Policy = "RequireStudentRole")]
        [HttpPost(ApiV1Routes.Order.AddToOrder)]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        public async Task<IActionResult> AddToOrder(string courseId, string userId)
        {
            var result = await _orderService.AddToOrderAsync(userId, courseId);
            if (result.Status)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [Authorize(Policy = "RequireStudentRole")]
        [HttpDelete(ApiV1Routes.Order.RemoveFromOrder)]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        public async Task<IActionResult> RemoveFromOrder(string courseId, string userId)
        {
            var result = await _orderService.RemoveFromOrderAsync(userId, courseId);
            if (result.Status)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }
}
