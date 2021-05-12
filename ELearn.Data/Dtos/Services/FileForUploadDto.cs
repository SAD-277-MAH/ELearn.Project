using System;
using System.Collections.Generic;
using System.Text;

namespace ELearn.Data.Dtos.Services
{
    public class FileForUploadDto
    {
        public bool Status { get; set; }

        public string Message { get; set; }

        public string Url { get; set; }

        public string PublicId { get; set; } = "Local Storage";
    }
}
