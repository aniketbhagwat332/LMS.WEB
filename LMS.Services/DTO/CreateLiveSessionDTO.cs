using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Services.DTO
{
    public class CreateLiveSessionDTO
    {
        public int CourseId { get; set; }

        public string Title { get; set; }

        public DateTime StartTime { get; set; }

        public int DurationMinutes { get; set; }
    }
}
