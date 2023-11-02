using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class AppUser: IdentityUser
    {
        public int Id
        {
            get;
            set;
        }
        public string? Name
        {
            get;
            set;
        }
        public string? Password
        {
            get;
            set;
        }
        public string? UserRole
        {
            get;
            set;
        }
        public bool? IsDeleted
        {
            get;
            set;
        }
    }
}
