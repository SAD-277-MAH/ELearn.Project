using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ELearn.Data.Dtos.Site.Order
{
    public class OrderDetailForDetailedDto
    {
        [Required]
        public string CourseId { get; set; }

        [Display(Name = "قیمت واحد")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        public int Price { get; set; }

        [Display(Name = "عنوان دوره")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "مقدار {0} نمی تواند بیشتر از {1} باشد")]
        public string Title { get; set; }

        [Display(Name = "تصویر دوره")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "مقدار {0} نمی تواند بیشتر از {1} باشد")]
        public string PhotoUrl { get; set; }
    }
}
