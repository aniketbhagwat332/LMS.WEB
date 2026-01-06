using LMS.Repository.Interfaces;
using LMS.Repository.Models;
using LMS.Repository.Repositories;
using LMS.Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Services.Services
{
    public class AdminService
    {
        private readonly IUserRepository _userRepo;
        private readonly ICourseRepository _courseRepo;
        private readonly ILessonRepository _lessonRepo;
        private readonly IEnrollmentRepository _enrollmentRepo;

        public AdminService(
            IUserRepository userRepo,
            ICourseRepository courseRepo,
            ILessonRepository lessonRepo,
            IEnrollmentRepository enrollmentRepo)
        {
            _userRepo = userRepo;
            _courseRepo = courseRepo;
            _lessonRepo = lessonRepo;
            _enrollmentRepo = enrollmentRepo;
        }

        // 📊 DASHBOARD STATS
        public async Task<AdminDashboardVM> GetDashboardStatsAsync()
        {
            return new AdminDashboardVM
            {
                TotalUsers = await _userRepo.CountAsync(),
                TotalStudents = await _userRepo.CountByRoleAsync("Student"),
                TotalInstructors = await _userRepo.CountByRoleAsync("Instructor"),
                TotalCourses = await _courseRepo.CountAsync(),
                TotalLessons = await _lessonRepo.CountAsync(),
                TotalEnrollments = await _enrollmentRepo.CountAsync()
            };
        }

        // 👥 USER MANAGEMENT
        //public async Task<List<User>> GetAllUsersAsync()
        //{
        //    return await _userRepo.GetAllUsersAsync();
        //}

        public async Task UpdateUserRoleAsync(int userId, string role)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found");

            user.Role = role;
            await _userRepo.UpdateAsync(user);
        }

        public async Task ToggleUserStatusAsync(int userId)
        {
            await _userRepo.ToggleUserStatusAsync(userId);
        }

        public async Task<List<Course>> GetPendingCoursesAsync()
        {
            return await _courseRepo.GetPendingCoursesAsync();
        }

        public async Task ApproveCourseAsync(int courseId)
        {
            await _courseRepo.UpdateCourseStatusAsync(courseId, "Approved");
        }

        public async Task RejectCourseAsync(int courseId)
        {
            await _courseRepo.UpdateCourseStatusAsync(courseId, "Rejected");
        }
        public async Task<AnalyticsVM> GetAnalyticsAsync()
        {
            return new AnalyticsVM
            {
                Students = await _userRepo.CountByRoleAsync("Student"),
                Instructors = await _userRepo.CountByRoleAsync("Instructor"),
                Admins = await _userRepo.CountByRoleAsync("Admin"),

                ApprovedCourses = await _courseRepo.CountByStatusAsync("Approved"),
                PendingCourses = await _courseRepo.CountByStatusAsync("Pending"),
                RejectedCourses = await _courseRepo.CountByStatusAsync("Rejected")
            };
        }

        public async Task<List<UserManagementDTO>> GetAllUsersAsync()
        {
            var users = await _userRepo.GetAllUsersAsync();

            return users.Select(u => new UserManagementDTO
            {
                UserId = u.UserId,
                FullName = u.FullName,
                Email = u.Email,
                Role = u.Role,
                IsActive = u.IsActive
            }).ToList();
        }
        public async Task<List<User>> GetPendingUsersAsync()
        {
            return await _userRepo.GetPendingUsersAsync();
        }

        public async Task ApproveUserAsync(int userId)
        {
            await _userRepo.ActivateUserAsync(userId);
            await _userRepo.SaveAsync();
        }

    }
}
