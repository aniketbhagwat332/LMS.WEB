using LMS.Repository.Models;
using LMS.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LMS.Web.Controllers
{
    public class CoursesController : Controller
    {
        private readonly CourseService _courseService;

        public CoursesController(CourseService courseService)
        {
            _courseService = courseService;
        }

        // Admin + Instructor only
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> Index()
        {
            var courses = await _courseService.GetAllCoursesAsync();
            return View(courses);
        }

        // Admin + Instructor only
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> Create()
        {
            var instructors = await _courseService.GetInstructorsAsync();
            ViewBag.Instructors = new SelectList(instructors, "UserId", "FullName");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Instructor")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Course course)
        {
            if (!ModelState.IsValid)
            {
                return View(course);
            }

            await _courseService.CreateCourseAsync(course);
            return RedirectToAction(nameof(Index));
        }

        // ✅ Student only
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Available()
        {
            var courses = await _courseService.GetApprovedCoursesAsync();
            return View(courses);
        }

    }
}
