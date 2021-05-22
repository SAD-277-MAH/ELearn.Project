using ELearn.Data.Dtos.Site.Users;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELearn.Data.Dtos.Site.Tokens
{
    public class TokenForLoginResponseDto
    {
        [JsonProperty(PropertyName = "token")]
        public string Token { get; set; }

        [JsonProperty(PropertyName = "refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty(PropertyName = "roles")]
        public IList<string> Roles { get; set; }

        [JsonProperty(PropertyName = "user")]
        public UserForDetailedDto User { get; set; }
    }
}
