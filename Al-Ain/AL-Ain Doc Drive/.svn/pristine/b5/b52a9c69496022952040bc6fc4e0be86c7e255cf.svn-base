using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Search_n_View.Models
{
    public class UserDepartment
    {
        [Key]
        public int UDpId { get; set; }

        [ForeignKey("User")]
        public string UID { get; set; }
        public virtual ApplicationUser User { get; set; }

        [ForeignKey("Department")]
        public int MdID { get; set; }
        public virtual MainDepartment Department { get; set; }
    }
}