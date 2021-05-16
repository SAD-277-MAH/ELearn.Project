using ELearn.Data.ReturnMessages;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELearn.Data.Dtos.Site.Order
{
    public class OrderForReturnDto
    {
        public OrderForReturnDto(OrderForDetailedDto order, Response response)
        {
            Order = order;
            Response = response;
        }

        public OrderForDetailedDto Order { get; set; }
        public Response Response { get; set; }
    }
}
