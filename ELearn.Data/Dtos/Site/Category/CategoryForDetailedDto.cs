using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ELearn.Data.Dtos.Site.Category
{
    public class CategoryForDetailedDto
    {
        [Display(Name = "نام دسته بندی")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "مقدار {0} نمی تواند بیشتر از {1} باشد")]
        public string Name { get; set; }

        [Display(Name = "نام سر دسته")]
        public string ParentName { get; set; }

        public int? ParentId { get; set; }
    }
}
