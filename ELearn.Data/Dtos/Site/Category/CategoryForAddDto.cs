using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ELearn.Data.Dtos.Site.Category
{
    public class CategoryForAddUpdateDto
    {
        [Display(Name = "نام دسته بندی")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "مقدار {0} نمی تواند بیشتر از {1} باشد")]
        public string Name { get; set; }

        [Required]
        public int ParentId { get; set; }
    }
}
