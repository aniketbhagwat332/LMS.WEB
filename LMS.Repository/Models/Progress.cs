using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Repository.Models
{
    public class Progress
    {
        public int ProgressId { get; set; }

        public int EnrollmentId { get; set; }
        public Enrollment Enrollment { get; set; }

        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }

        public bool IsCompleted { get; set; }
    }
}
