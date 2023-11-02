using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Search_n_View.Models
{
    public class AccessGroup : BaseModel
    {
        public string Name { get; set; }
        public bool CanView { get; set; }
        public bool CanAddNote { get; set; }
        public bool CanPrint { get; set; }
        public bool CanDownload { get; set; }
        public bool CanUpload { get; set; }
        public bool CanEmail { get; set; }
        public string AgShortName { get; set; }

    }
}