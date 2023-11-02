using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Search_n_View.Models
{
    public class ItemType : BaseModel
    {
        public string Name { get; set; }


        [ForeignKey("MainDepartment")]
        public int MdId { get; set; }
        public virtual MainDepartment MainDepartment { get; set; }
    }
}