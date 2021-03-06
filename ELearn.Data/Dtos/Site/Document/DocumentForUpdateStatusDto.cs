using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ELearn.Data.Dtos.Site.Document
{
    public class DocumentForUpdateStatusDto
    {
        [Display(Name = "وضعیت")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        [Range(-1, 1, ErrorMessage = "مقدار {0} باید 1-، 0 یا 1 باشد")]
        public int Status { get; set; }

        [Display(Name = "پیام")]
        [MaxLength(200, ErrorMessage = "مقدار {0} نمی تواند بیشتر از {1} باشد")]
        public string Message { get; set; }
    }
}
