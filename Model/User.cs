﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independiente.Model
{

    public enum UserRole
    {
        Advisor,
        Analyst,
        Collector,
        Administrator
    }
    public class User
    {
        public int EmployeeId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }

        public UserRole UserRole { get; set; }
    }
}

