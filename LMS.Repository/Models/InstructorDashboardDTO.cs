using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Repository.Models
{
    public class InstructorDashboardDTO
    {
        public int CourseId { get; set; }
        public string CourseTitle { get; set; }

        public int LessonCount { get; set; }
        public int EnrollmentCount { get; set; }

        public string Status { get; set; } // Draft / Published (future use)
    }
}
