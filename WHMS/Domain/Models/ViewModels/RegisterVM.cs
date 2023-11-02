﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ViewModels
{
    public class RegisterVM
    {
        public string? Name
        {
            get;
            set;
        }
        public string? Email
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
