using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Repository.Models
{
    public class AdminDashboardVM
    {
        public int TotalUsers { get; set; }
        public int TotalStudents { get; set; }
        public int TotalInstructors { get; set; }
        public int TotalCourses { get; set; }
        public int TotalLessons { get; set; }
        public int TotalEnrollments { get; set; }
    }
}
