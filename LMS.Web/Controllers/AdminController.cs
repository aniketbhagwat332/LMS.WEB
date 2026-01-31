using LMS.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace LMS.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AdminService _adminService;

        public AdminController(AdminService adminService)
        {
            _adminService = adminService;
        }
        // 📊 ADMIN DASHBOARD Aniket Bhagwat
        public async Task<IActionResult> Dashboard()
        {
            var stats = await _adminService.GetDashboardStatsAsync();
            return View(stats);
        }

        // 👥 USER ROLE MANAGEMENT
        public async Task<IActionResult> Users()
        {
            var users = await _adminService.GetAllUsersAsync();
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRole(int userId, string role)
        {
            await _adminService.UpdateUserRoleAsync(userId, role);
            return RedirectToAction(nameof(Users));
        }
        [HttpPost]
        public async Task<IActionResult> ToggleUser(int userId)
        {
            await _adminService.ToggleUserStatusAsync(userId);
            return RedirectToAction(nameof(Users));
        }

        public async Task<IActionResult> PendingCourses()
        {
            var courses = await _adminService.GetPendingCoursesAsync();
            return View(courses);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveCourse(int courseId)
        {
            await _adminService.ApproveCourseAsync(courseId);
            return RedirectToAction(nameof(PendingCourses));
        }

        [HttpPost]
        public async Task<IActionResult> RejectCourse(int courseId)
        {
            await _adminService.RejectCourseAsync(courseId);
            return RedirectToAction(nameof(PendingCourses));
        }
        public async Task<IActionResult> Analytics()
        {
            var data = await _adminService.GetAnalyticsAsync();
            return View(data);
        }
        public async Task<IActionResult> PendingUsers()
        {
            var users = await _adminService.GetPendingUsersAsync();
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveUser(int userId)
        {
            await _adminService.ApproveUserAsync(userId);
            return RedirectToAction(nameof(PendingUsers));
        }

    }
}