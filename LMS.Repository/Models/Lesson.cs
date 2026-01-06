using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Repository.Models
{
    public class Lesson
    {
        [Key]
        public int LessonId { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public string LessonTitle { get; set; }

        public int OrderNo { get; set; }
    }
}
