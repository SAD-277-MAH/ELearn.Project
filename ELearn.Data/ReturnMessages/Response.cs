using System;
using System.Collections.Generic;
using System.Text;

namespace ELearn.Data.ReturnMessages
{
    public class Response
    {
        public Response()
        {
            Messages = new List<string>();
        }
        public Response(bool status, string message)
        {
            Status = status;
            Messages = new List<string>();
            Messages.Add(message);
        }
        public Response(bool status, List<string> messages)
        {
            Status = status;
            Messages = new List<string>();
            Messages = messages;
        }

        public bool Status { get; set; }
        public List<string> Messages { get; set; }
    }
}
