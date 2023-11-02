using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Search_n_View.Models
{
    public class Notes : BaseModel
    {
        public string Note { get; set; }

        [ForeignKey("User")]
        public string UId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [ForeignKey("Document")]
        public int DId { get; set; }
        public virtual Document Document { get; set; }
  
        
    }
}