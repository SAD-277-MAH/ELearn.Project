using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ELearn.Data.Models
{
    public class UserCourse : BaseEntity<string>
    {
        public UserCourse()
        {
            Id = Guid.NewGuid().ToString();
            DateCreated = DateTime.Now;
            DateModified = DateTime.Now;
        }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string CourseId { get; set; }


        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; }
    }
}
