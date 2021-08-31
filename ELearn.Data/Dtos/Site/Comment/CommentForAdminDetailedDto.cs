using System;
using System.Collections.Generic;
using System.Text;

namespace ELearn.Data.Dtos.Site.Comment
{
    public class CommentForAdminDetailedDto
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhotoUrl { get; set; }

        public string Date { get; set; }

        public string Text { get; set; }

        public int Status { get; set; }

        public string CourseId { get; set; }

        public string CourseTitle { get; set; }
    }
}
