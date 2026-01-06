using LMS.Repository.Models;
using LMS.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Web.Controllers
{
    [Authorize(Roles = "Instructor")]
    public class InstructorController : Controller
    {
        private readonly CourseService _courseService;

        public InstructorController(CourseService courseService)
        {
            _courseService = courseService;
        }

        public async Task<IActionResult> Dashboard()
        {
            int instructorId = int.Parse(User.FindFirst("UserId")!.Value);

            var dashboardData = await _courseService.GetInstructorDashboardAsync(instructorId);

            return View(dashboardData);
        }
        public IActionResult CreateCourse()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> CreateCourse(Course course)
        {
            if (!ModelState.IsValid)
                return View(course);

            int instructorId = int.Parse(User.FindFirst("UserId")!.Value);

            course.InstructorId = instructorId; // 🔒 enforced
            await _courseService.CreateCourseAsync(course);

            return RedirectToAction("Dashboard");
        }

    }
}

