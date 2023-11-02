using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Search_n_View.Models;

namespace Search_n_View.ViewModels
{
    public class ManageDriveViewModel
    {
        public List<DriveViewModel> Drive { get; set; }
    }


    public class DriveViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please Enter DeiveLetter.")]
        public string BasePath { get; set; }

        public bool IsActive { get; set; }
        public string AddedBy { get; set; }
        public DateTime AddedTime { get; set; }

        public string ModifiedBy { get; set; }
        public DateTime ModifiedTime { get; set; }
    }
}