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
    public class CourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IUserRepository _userRepository;
        public CourseService(
           ICourseRepository courseRepository,
           IUserRepository userRepository)
        {
            _courseRepository = courseRepository;
            _userRepository = userRepository;
        }

        public Task<List<Course>> GetAllCoursesAsync()
            => _courseRepository.GetCoursesWithInstructorAsync();


        public Task<List<User>> GetInstructorsAsync()
            => _userRepository.GetInstructorsAsync();

        //public async Task CreateCourseAsync(Course course)
        //{
        //    // Business rules go here
        //    await _courseRepository.AddAsync(course);
        //    await _courseRepository.SaveAsync();
        //}
        public async Task<List<Course>> GetCoursesByInstructorAsync(int instructorId)
        {
            return await _courseRepository
                .GetCoursesByInstructorAsync(instructorId);
        }

        public async Task CreateCourseAsync(Course course)
        {
            course.Status = "Pending"; // 🔒 Instructor cannot auto-approve

            await _courseRepository.AddAsync(course);
            await _courseRepository.SaveAsync();
        }
        public Task<List<Course>> GetApprovedCoursesAsync()
        {
            return _courseRepository.GetApprovedCoursesAsync();
        }

        public async Task<List<InstructorDashboardDTO>> GetInstructorDashboardAsync(int instructorId)
        {
            var courses = await _courseRepository
                .GetCoursesByInstructorAsync(instructorId);

            return courses.Select(c => new InstructorDashboardDTO
            {
                CourseId = c.CourseId,
                CourseTitle = c.CourseTitle,
                LessonCount = c.Lessons.Count,
                EnrollmentCount = c.Enrollments.Count,
                Status = "Draft" // default for now
            }).ToList();
        }
        public async Task<Course?> GetCourseDetailAsync(int courseId, int userId, string role)
        {
            var course = await _courseRepository.GetByIdAsync(courseId);

            if (course == null)
                return null;

            if (role == "Instructor" && course.InstructorId != userId)
                throw new UnauthorizedAccessException();

            return course;
        }
        public async Task UpdateCourseAsync(
            UpdateCourseDTO dto,
            int userId,
            string role)
        {
            var course = await _courseRepository.GetByIdAsync(dto.CourseId);

            if (course == null)
                throw new Exception("Course not found");

            if (role == "Instructor" && course.InstructorId != userId)
                throw new UnauthorizedAccessException();

            if (course.Status == "Approved" && role == "Instructor")
                throw new Exception("Approved courses can only be edited by Admin");

            course.CourseTitle = dto.CourseTitle;
            course.Description = dto.Description;

            _courseRepository.Update(course);
            await _courseRepository.SaveAsync();
        }


    }
}
