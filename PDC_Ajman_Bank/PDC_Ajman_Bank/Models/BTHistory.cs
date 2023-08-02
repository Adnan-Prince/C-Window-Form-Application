using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDC_Ajman_Bank.Models
{
    class BTHistory
    {
        public long SerialNo { get; set; }
        public double? InstAmt { get; set; }
        public DateTime EntryDate { get; set; }
        public string CheckNo { get; set; }
        public string Micr { get; set; }
        public int ChkStatusIdOld { get; set; }
        public int ChkStatusIdNew { get; set; }
    }
}
