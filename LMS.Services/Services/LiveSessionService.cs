using LMS.Repository.Interfaces;
using LMS.Repository.Models;
using LMS.Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Services.Services
{
    public class LiveSessionService
    {
        private readonly ICourseRepository _courseRepo;
        private readonly IRepository<LiveSession> _liveSessionRepo;
        private readonly ZoomMeetingService _zoomMeetingService;

        public LiveSessionService(
            ICourseRepository courseRepo,
            IRepository<LiveSession> liveSessionRepo,
            ZoomMeetingService zoomMeetingService)
        {
            _courseRepo = courseRepo;
            _liveSessionRepo = liveSessionRepo;
            _zoomMeetingService = zoomMeetingService;
        }

        public async Task<bool> ScheduleLiveSessionAsync(
            int courseId,
            int instructorId,
            string title,
            DateTime startTime,
            int durationMinutes)
        {
            // 1️⃣ Validate course ownership
            var course = await _courseRepo.GetByIdAsync(courseId);

            if (course == null || course.InstructorId != instructorId)
                return false; // ❌ Not allowed

            // 2️⃣ Validate business rules
            if (startTime <= DateTime.Now)
                throw new Exception("Live session must be scheduled in the future");

            if (durationMinutes <= 0)
                throw new Exception("Duration must be greater than zero");

            // 3️⃣ Create Zoom meeting
            var zoomResult = await _zoomMeetingService
                .CreateMeetingAsync(title, startTime, durationMinutes);

            // 4️⃣ Create LiveSession entity
            var liveSession = new LiveSession
            {
                CourseId = courseId,
                InstructorId = instructorId,
                Title = title,
                StartTime = startTime,
                DurationMinutes = durationMinutes,
                ZoomMeetingId = zoomResult.MeetingId,
                JoinUrl = zoomResult.JoinUrl,
                StartUrl = zoomResult.StartUrl,
                Status = "Scheduled"
            };

            // 5️⃣ Save to database
            await _liveSessionRepo.AddAsync(liveSession);
            await _liveSessionRepo.SaveAsync();

            return true;
        }
        public async Task<List<LiveSession>> GetLiveSessionsForCourseAsync(
    int courseId,
    int instructorId)
        {
            var course = await _courseRepo.GetByIdAsync(courseId);

            if (course == null || course.InstructorId != instructorId)
                throw new UnauthorizedAccessException();

            return await _liveSessionRepo
                .WhereAsync(ls => ls.CourseId == courseId);
        }
        public async Task<List<LiveSession>> GetSessionsByCourseAsync(int courseId)
        {
            return await _liveSessionRepo
                .WhereAsync(ls => ls.CourseId == courseId);
        }
        public async Task ScheduleLiveSessionAsync(
    CreateLiveSessionDTO model,
    int instructorId)
        {
            var session = new LiveSession
            {
                CourseId = model.CourseId,
                InstructorId = instructorId,
                Title = model.Title,
                StartTime = model.StartTime,
                DurationMinutes = model.DurationMinutes,
                Status = "Scheduled"
            };

            await _liveSessionRepo.AddAsync(session);
            await _liveSessionRepo.SaveAsync();
        }


    }
}

