using LMS.Repository.Models;
using LMS.Services.DTO;
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
        [HttpGet]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> Edit(int courseId)
        {
            int userId = int.Parse(User.FindFirst("UserId")!.Value);
            string role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)!.Value;

            var course = await _courseService
                .GetCourseDetailAsync(courseId, userId, role);

            if (course == null)
                return NotFound();

            return View(new UpdateCourseDTO
            {
                CourseId = course.CourseId,
                CourseTitle = course.CourseTitle,
                Description = course.Description
            });
        }
        [HttpPost]
        [Authorize(Roles = "Admin,Instructor")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateCourseDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                int userId = int.Parse(User.FindFirst("UserId")!.Value);
                string role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)!.Value;

                await _courseService.UpdateCourseAsync(model, userId, role);

                return RedirectToAction("Index");
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }
    }
}
