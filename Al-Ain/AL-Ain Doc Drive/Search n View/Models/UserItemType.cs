using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Search_n_View.Models
{
    public class UserItemType
    {
        [Key]
        public int UItId { get; set; }

        [ForeignKey("User")]
        public string UID { get; set; }
        public virtual ApplicationUser User { get; set; }

        [ForeignKey("ItemType")]
        public int ItID { get; set; }
        public virtual ItemType ItemType { get; set; }
    }
}