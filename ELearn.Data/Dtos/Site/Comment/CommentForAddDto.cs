using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ELearn.Data.Dtos.Site.Comment
{
    public class CommentForAddDto
    {
        [Display(Name = "متن پیام")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = "مقدار {0} نمی تواند بیشتر از {1} باشد")]
        public string Text { get; set; }
    }
}
