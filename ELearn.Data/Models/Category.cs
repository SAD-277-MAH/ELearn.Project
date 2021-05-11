using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ELearn.Data.Models
{
    public class Category : BaseEntity<int>
    {
        public Category()
        {
            DateCreated = DateTime.Now;
            DateModified = DateTime.Now;
        }


        [Display(Name = "نام دسته بندی")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "مقدار {0} نمی تواند بیشتر از {1} باشد")]
        public string Name { get; set; }

        public int? ParentId { get; set; }


        [ForeignKey("ParentId")]
        public virtual Category Parent { get; set; }


        public virtual ICollection<Category> Categories { get; set; }
    }
}
