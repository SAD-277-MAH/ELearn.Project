using System;
using System.Collections.Generic;
using System.Text;

namespace ELearn.Data.Dtos.Site.Users
{
    public class TeacherForAdminDetailedDto
    {
        public string UserId { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string NationalCode { get; set; }

        public string PhotoUrl { get; set; }

        public DateTime RegisterDate { get; set; }

        public int Status { get; set; }
    }
}
