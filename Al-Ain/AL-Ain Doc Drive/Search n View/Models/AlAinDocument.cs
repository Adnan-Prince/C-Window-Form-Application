﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Search_n_View.Models
{
    public class AlAinDocument:BaseModel
    {
        public string DocPath { get; set; }

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

        [ForeignKey("MainDepartment")]
        public int? MdId { get; set; }
        public virtual MainDepartment MainDepartment { get; set; }

        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string ApprovedBY { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string RejectedBy { get; set; }
        public DateTime? RejectedDate { get; set; }
        public string ScannedFilePath { get; set; }
        public string ChannelName { get; set; }
        public string Extension { get; set; }
        public string Pharmacy { get; set; }
        public string PharmacyBranch { get; set; }
        public bool IsActive { get; set; }
        public bool IsApproved { get; set; }
        public string BaseDrivePath { get; set; }
        public DateTime? EntryDate { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int PageCount { get; set; }
        public string BatchNo { get; set; }


    }
}