using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ELearn.Data.Dtos.Site.Course
{
    public class CourseForUpdateDto
    {
        [Display(Name = "عنوان دوره")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "مقدار {0} نمی تواند بیشتر از {1} باشد")]
        public string Title { get; set; }

        [Display(Name = "توضیحات دوره")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        public string Description { get; set; }

        [Display(Name = "قیمت")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        [Range(0, int.MaxValue, ErrorMessage = "مقدار {0} نمی تواند کمتر از {1} باشد")]
        public int Price { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public IFormFile File { get; set; }
    }
}
