using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ELearn.Data.Models
{
    public class OrderDetail : BaseEntity<string>
    {
        public OrderDetail()
        {
            Id = Guid.NewGuid().ToString();
            DateCreated = DateTime.Now;
            DateModified = DateTime.Now;
        }

        [Display(Name = "قیمت واحد")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        public int Price { get; set; }

        [Required]
        public string OrderId { get; set; }

        [Required]
        public string CourseId { get; set; }


        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }

        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; }
    }
}
