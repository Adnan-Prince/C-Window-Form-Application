using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ViewModels
{
    public class LoginVM
    {
        //public string? Id { get; set;}
        public string? UserName
        {
            get;
            set;
        }
        public string? Password
        {
            get;
            set;
        }
    }
}
