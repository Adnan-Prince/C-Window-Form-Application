using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDC_Ajman_Bank.Models
{
    class BeneficiaryTransaction
    {
        public long SerialNo { get; set; }

        // Primary Key
        public string CIF { get; set; }

        // Primary Key
        public string BeneficiaryAccountNo { get; set; }

        // Other Columns
        public double? InstAmt { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime? StoreInDate { get; set; }
        public DateTime? StoreOutDate { get; set; }
        public DateTime? SentBackDate { get; set; }
        public DateTime DueDate { get; set; }
        public string CheckNo { get; set; }
        public string BankId { get; set; }
        public int? BranchId { get; set; }
        public int ChkStatusId { get; set; }
        public int? CityId { get; set; }
        public string CBC { get; set; }
        public string BeneficiaryBarcode { get; set; }
        public string Micr { get; set; }
        public string DraweeName { get; set; }
        public string DraweeAccount { get; set; }
        public string Comments { get; set; }
        public DateTime? DepositeDate { get; set; }
        public string UnitNo { get; set; }
        public string BuildingName { get; set; }
        public string Location { get; set; }
        public string ChecqueType { get; set; }
    }
}
