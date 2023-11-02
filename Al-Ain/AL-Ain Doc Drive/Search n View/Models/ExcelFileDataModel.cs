using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Search_n_View.Models
{
    public class ExcelFileDataModel
    {
        public string DocPath { get; set; }
        public string PageCount { get; set; }
        public string BatchNo { get; set; }
        public string InvoiceNo { get; set; }
        public string PharmacyBranch { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string Pharmacy { get; set; }
    }
}