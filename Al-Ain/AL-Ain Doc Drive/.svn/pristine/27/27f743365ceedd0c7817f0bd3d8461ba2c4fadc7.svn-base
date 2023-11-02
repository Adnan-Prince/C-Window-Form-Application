using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Search_n_View.Models
{
    public class DocumentType : BaseModel
    {
        public string Name { get; set; }

        [ForeignKey("ItemType")]
        public int? ItID { get; set; }
        public virtual ItemType ItemType { get; set; }
    }
}