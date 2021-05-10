using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ELearn.Data.Models
{
    public class Token : BaseEntity<string>
    {
        public Token()
        {
            Id = Guid.NewGuid().ToString();

            DateCreated = DateTime.Now;

            DateModified = DateTime.Now;
        }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string ClientId { get; set; }

        [Required]
        public string IP { get; set; }

        [Required]
        public string Value { get; set; }

        [Required]
        public DateTime ExpireTime { get; set; }


        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
