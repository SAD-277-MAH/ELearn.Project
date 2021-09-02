using ELearn.Data.Dtos.Site.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELearn.Data.Dtos.Site.Document
{
    public class DocumentForAdminCompleteDetailedDto
    {
        public UserForTeacherDetailedDto Teacher { get; set; }
        public List<DocumentForAdminDetailedDto> Document { get; set; }
    }
}
