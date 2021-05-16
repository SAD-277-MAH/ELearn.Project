using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ELearn.Data.Dtos.Site.Order
{
    public class OrderForDetailedDto
    {
        [Display(Name = "جمع فاکتور")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        public int OrderSum { get; set; }

        [Display(Name = "تخفیف")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        public int Discount { get; set; }


        public List<OrderDetailForDetailedDto> OrderDetails { get; set; }
    }
}
