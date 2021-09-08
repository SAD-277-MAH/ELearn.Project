using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ELearn.Data.Dtos.Site.Users
{
    public class UserForGetUserNameDto
    {
        [Required]
        public string UserName { get; set; }
    }
}
