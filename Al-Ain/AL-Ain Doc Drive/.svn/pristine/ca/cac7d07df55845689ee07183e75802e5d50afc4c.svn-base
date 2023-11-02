using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Search_n_View.Models
{
    public class Document: BaseModel
    {
        public string DocPath { get; set; }
        public string ClaimNo { get; set; }    

        [ForeignKey("ItemType")]
        public int ItID { get; set; }
        public virtual ItemType ItemType { get; set; }

        [ForeignKey("DocumentType")]
        public int DtId { get; set; }
        public virtual DocumentType DocumentType { get; set; }

        [ForeignKey("EmailAttachment")]
        public int? DEId { get; set; }
        public virtual EmailAttachment EmailAttachment { get; set; }

        [ForeignKey("User")]
        public string UId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public string PolicyNo { get; set; }

        public string Email { get; set; }

        public string ScannedFilePath { get; set; }
        public string Year { get; set; }
        public string Extension { get; set; }
        public bool? IsActive { get; set; }
        public string BaseDrivePath { get; set; }

        public DateTime? Date { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

    }
}