using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Search_n_View.Models
{
    public class DocumentHistory : BaseModel
    {
        public bool Viewed { get; set; }
        public bool AddedNote { get; set; }
        public bool Printed { get; set; }
        public bool Scanned { get; set; }
        public bool Downloaded { get; set; }
        public bool Uploaded { get; set; }
        public bool Emailed { get; set; }
        public string EmailRecipients { get; set; }
        public string EmailBccRecipients { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
        public string UserNote { get; set; }

        [ForeignKey("User")]
        public string UId { get; set; }
        public virtual ApplicationUser User { get; set; }


        [ForeignKey("AlAinDocument")]
        public int DId { get; set; }
        public virtual AlAinDocument AlAinDocument { get; set; }
    }
}