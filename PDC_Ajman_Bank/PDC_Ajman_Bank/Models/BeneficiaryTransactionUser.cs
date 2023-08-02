using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDC_Ajman_Bank.Models
{
    class BeneficiaryTransactionUser
    {
        public string CIF { get; set; }

        // Primary Key
        public string CheckNo { get; set; }

        // Primary Key
        public string BankId { get; set; }

        // Primary Key
        public string UserId { get; set; }

        // Other Columns
        public DateTime TrDate { get; set; }
        public DateTime TrTime { get; set; }
        public int ChkStatusId { get; set; }
        public long PrintBatchNo { get; set; }
        public DateTime? PrintDate { get; set; }
        public string Micr { get; set; }
    }
}
