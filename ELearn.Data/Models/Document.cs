using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ELearn.Data.Models
{
    public class Document : BaseEntity<string>
    {
        public Document()
        {
            Id = Guid.NewGuid().ToString();
            DateCreated = DateTime.Now;
            DateModified = DateTime.Now;
        }

        [Display(Name = "آدرس مدرک کاربر")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = "مقدار {0} نمی تواند بیشتر از {1} باشد")]
        public string FileUrl { get; set; }

        [Display(Name = "پیام")]
        [MaxLength(200, ErrorMessage = "مقدار {0} نمی تواند بیشتر از {1} باشد")]
        public string Message { get; set; }

        [Required]
        public string TeacherId { get; set; }


        [ForeignKey("TeacherId")]
        public virtual User Teacher { get; set; }
    }
}
