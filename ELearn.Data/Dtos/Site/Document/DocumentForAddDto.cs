using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ELearn.Data.Dtos.Site.Document
{
    public class DocumentForAddDto
    {
        [Required]
        public IFormFile File { get; set; }
    }
}
