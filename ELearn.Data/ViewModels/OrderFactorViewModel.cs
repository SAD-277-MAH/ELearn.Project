using System;
using System.Collections.Generic;
using System.Text;

namespace ELearn.Data.ViewModels
{
    public class OrderFactorViewModel
    {
        public OrderFactorViewModel(bool status, int price, string message)
        {
            Status = status;
            Price = price;
            Message = message;
        }

        public bool Status { get; set; }
        public int Price { get; set; }
        public string Message { get; set; }
    }
}
