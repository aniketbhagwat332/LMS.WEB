using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Repository.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Role { get; set; } // Admin, Instructor, Student

        // ✅ ADD THIS
        public bool IsActive { get; set; } = true;
    }
}
