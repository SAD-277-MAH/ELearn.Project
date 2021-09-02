using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ELearn.Data.Dtos.Site.Document
{
    public class DocumentForAdminDetailedDto
    {
        public string FileUrl { get; set; }

        public string Message { get; set; }

        public int Status { get; set; }

        public string TeacherId { get; set; }
    }
}
