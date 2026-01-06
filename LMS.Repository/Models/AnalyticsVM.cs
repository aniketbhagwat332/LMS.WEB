using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Repository.Models
{
    public class AnalyticsVM
    {
        // User Distribution
        public int Students { get; set; }
        public int Instructors { get; set; }
        public int Admins { get; set; }

        // Course Status
        public int ApprovedCourses { get; set; }
        public int PendingCourses { get; set; }
        public int RejectedCourses { get; set; }
    }

}
