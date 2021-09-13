using System;
using System.Collections.Generic;
using System.Text;

namespace ELearn.Data.Dtos.Site.Exam
{
    public class ExamForStatusDto
    {
        public int Grade { get; set; }

        public int PassingGrade { get; set; }

        public int MaxGrade { get; set; }

        public bool Status { get; set; }
    }
}
