using ELearn.Data.Dtos.Site.ExamQuestion;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELearn.Data.Dtos.Site.Exam
{
    public class ExamForDetailedDto
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int PassingGrade { get; set; }

        public string SessionId { get; set; }


        public List<ExamQuestionForDetailedDto> ExamQuestions { get; set; }
    }
}
