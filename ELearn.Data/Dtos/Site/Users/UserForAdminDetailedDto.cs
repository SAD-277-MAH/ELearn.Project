using System;
using System.Collections.Generic;
using System.Text;

namespace ELearn.Data.Dtos.Site.Users
{
    public class UserForAdminDetailedDto
    {
        public string Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalCode { get; set; }
    }
}
