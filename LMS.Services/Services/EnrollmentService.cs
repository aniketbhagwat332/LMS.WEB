using LMS.Repository.Interfaces;
using LMS.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Services.Services
{
    public class EnrollmentService
    {
        private readonly IEnrollmentRepository _enrollmentRepo;
        private readonly IProgressRepository _progressRepo;
        private readonly ILessonRepository _lessonRepo;


        public EnrollmentService(IEnrollmentRepository enrollmentRepo,IProgressRepository progressRepo,ILessonRepository lessonRepo)
        {
            _enrollmentRepo = enrollmentRepo;
            _progressRepo = progressRepo;
            _lessonRepo = lessonRepo;
        }


        public async Task<bool> EnrollStudentAsync(int studentId, int courseId)
        {
            if (await _enrollmentRepo.IsStudentEnrolledAsync(studentId, courseId))
                return false;

            var enrollment = new Enrollment
            {
                StudentId = studentId,
                CourseId = courseId,
                EnrollDate = DateTime.UtcNow
            };

            await _enrollmentRepo.AddAsync(enrollment);
            await _enrollmentRepo.SaveAsync();
            return true;
        }

        public async Task<List<Enrollment>> GetStudentEnrollmentsAsync(int studentId)
        {
            return await _enrollmentRepo.GetEnrollmentsByStudentAsync(studentId);
        }
        public async Task<int> GetCourseProgressPercentageAsync(int enrollmentId, int courseId)
        {
            int totalLessons = await _lessonRepo.GetLessonCountByCourseAsync(courseId);

            if (totalLessons == 0)
                return 0;

            int completedLessons =
                await _progressRepo.GetCompletedLessonCountAsync(enrollmentId);

            return (completedLessons * 100) / totalLessons;
        }

    }
}
