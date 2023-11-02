using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Search_n_View.Models
{
    public class ErrorLog : BaseModel
    {
        public string ClassName { get; set; }
        public string FunctionName { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }
        public string Stacktrace { get; set; }
    }
}