using System;
using System.Collections.Generic;
using System.Text;

namespace ELearn.Data.Common.ReturnMessages
{
    public class ErrorList
    {
        public ErrorList()
        {
            Errors = new List<string>();
        }

        public ICollection<string> Errors { get; set; }
    }
}
