using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Repository.Models
{
    public class Course
    {
        public int CourseId { get; set; }

        [Required]
        public string CourseTitle { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int InstructorId { get; set; }

        [ForeignKey(nameof(InstructorId))]
        [ValidateNever] // 🔥 IMPORTANT
        public User Instructor { get; set; }

        public string Status { get; set; } = "Pending";

        public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
