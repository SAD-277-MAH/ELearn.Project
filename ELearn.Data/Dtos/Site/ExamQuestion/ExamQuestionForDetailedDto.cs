using System;
using System.Collections.Generic;
using System.Text;

namespace ELearn.Data.Dtos.Site.ExamQuestion
{
    public class ExamQuestionForDetailedDto
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string FileUrl { get; set; }

        public string FirstChoice { get; set; }

        public string SecondChoice { get; set; }

        public string ThirdChoice { get; set; }

        public string FourthChoice { get; set; }

        public short CorrectAnswer { get; set; }

        public string ExamId { get; set; }
    }
}
