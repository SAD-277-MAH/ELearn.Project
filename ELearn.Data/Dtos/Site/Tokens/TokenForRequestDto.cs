using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ELearn.Data.Dtos.Site.Tokens
{
    public class TokenForRequestDto
    {
        [Required]
        public string GrantType { get; set; }

        public string ClientId { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده صحیح نمی‌باشد")]
        public string UserName { get; set; }

        public string Password { get; set; }

        public string RefreshToken { get; set; }

        public bool IsRemember { get; set; } = false;
    }
}
