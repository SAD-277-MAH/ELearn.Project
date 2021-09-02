using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ELearn.Data.Dtos.Site.Document
{
    public class DocumentForDetailedDto
    {
        public string FileUrl { get; set; }

        public string Message { get; set; }

        public int Status { get; set; }
    }
}
