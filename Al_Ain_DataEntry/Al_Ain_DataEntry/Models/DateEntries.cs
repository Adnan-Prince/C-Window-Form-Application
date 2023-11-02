using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Al_Ain_DataEntry.Models
{
    public class DataEntries
    {
        public int ID { get; set; } // Primary key, auto-incremented
        public string BranchName { get; set; }
        public string BagNumber  { get; set; }
        public string Date { get; set; }
        public string SubmissionDate { get; set; }
        public DateTime EntryDate { get; set; }
        public string BoxNumber { get; set; }
        public string FileBarcode { get; set; }

        public string UserName {get; set;}
        public bool IsComplete { get; set; } // New boolean column
    }
}
