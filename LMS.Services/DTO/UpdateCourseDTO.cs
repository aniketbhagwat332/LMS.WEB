using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Services.DTO
{
    public class UpdateCourseDTO
    {
        public int CourseId { get; set; }
        public string CourseTitle { get; set; }
        public string Description { get; set; }
    }
}
