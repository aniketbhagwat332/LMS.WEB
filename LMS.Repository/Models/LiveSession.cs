using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Repository.Models
{
    public class LiveSession
    {
        public int LiveSessionId { get; set; }

        // 🔗 Relations
        public int CourseId { get; set; }
        public Course Course { get; set; }

        public int InstructorId { get; set; }
        public User Instructor { get; set; }

        // 📘 Session info
        public string Title { get; set; }
        public DateTime StartTime { get; set; }
        public int DurationMinutes { get; set; }

        // 🎥 Zoom info
        public string? ZoomMeetingId { get; set; }
        public string? JoinUrl { get; set; }
        public string? StartUrl { get; set; }

        // 📌 Status control
        public string Status { get; set; } = "Scheduled";
    }
}
