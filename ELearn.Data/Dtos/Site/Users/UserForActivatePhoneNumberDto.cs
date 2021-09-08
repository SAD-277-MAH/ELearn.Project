using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ELearn.Data.Dtos.Site.Users
{
    public class UserForActivatePhoneNumberDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Token { get; set; }
    }
}
