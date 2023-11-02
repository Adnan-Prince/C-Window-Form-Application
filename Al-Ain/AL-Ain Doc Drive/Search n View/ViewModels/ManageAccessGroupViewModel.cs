using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Search_n_View.ViewModels
{
    public class ManageAccessGroupViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string AgShortName { get; set; }
        public bool CanView { get; set; }
        public bool CanUpload { get; set; }
        public bool CanDownload { get; set; }
        public bool CanEmail { get; set; }
        public bool CanPrint { get; set; }
        public bool CanAddNote { get; set;}
        public string AddedBy { get; set; }
        public DateTime AddedTime {get; set;}
    }

    public class EditAccessGroupViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [Display(Name = "Code")]
        public string AgShortName { get; set; }
        public bool CanView { get; set; }
        public bool CanUpload { get; set; }
        public bool CanDownload { get; set; }
        public bool CanEmail { get; set; }
        public bool CanPrint { get; set; }
        public bool CanAddNote { get; set; }
        public DateTime AddedTime { get; set; }
    }

    public class EditAccessGroupCodeViewModel
    {
      
        [Required]
        public string Name { get; set; }
        public string AgShortName { get; set; }
    }

}