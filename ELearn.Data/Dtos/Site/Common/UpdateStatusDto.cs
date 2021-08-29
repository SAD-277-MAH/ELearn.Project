using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ELearn.Data.Dtos.Site.Common
{
    public class UpdateStatusDto
    {
        [Display(Name = "وضعیت دوره")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        [Range(-1, 1, ErrorMessage = "مقدار {0} باید 1-، 0 یا 1 باشد")]
        public int Status { get; set; }
    }
}
