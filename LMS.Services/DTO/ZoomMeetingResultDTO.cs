using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Services.DTO
{
    public class ZoomMeetingResult
    {
        public string MeetingId { get; set; }
        public string JoinUrl { get; set; }
        public string StartUrl { get; set; }
    }
}
