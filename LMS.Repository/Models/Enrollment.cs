using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Repository.Models
{
    public class Enrollment
    {
        public int EnrollmentId { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public int StudentId { get; set; }
        public User Student { get; set; }

        public DateTime EnrollDate { get; set; }
    }
}
