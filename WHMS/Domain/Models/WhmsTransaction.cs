using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class WhmsTransaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WOID { get; set; }
        public required string BoxNo { get; set;}
        public required string ClientName { get; set;}
        public required int StatusId { get; set;}
        public required int RoleId { get; set;}
        public required string UserName { get; set;}
        public required DateTime TimeIn { get; set;}
        public required DateTime TimeOut { get; set;}
    }
}
