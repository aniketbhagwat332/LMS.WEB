using LMS.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Web.Controllers
{
    [Authorize(Roles = "Student")]
    public class EnrollmentsController : Controller
    {
        private readonly EnrollmentService _enrollmentService;

        public EnrollmentsController(EnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        [HttpPost]
        public async Task<IActionResult> Enroll(int courseId)
        {
            int studentId = int.Parse(User.FindFirst("UserId")!.Value);

            bool success = await _enrollmentService.EnrollStudentAsync(studentId, courseId);

            TempData["Message"] = success
                ? "Enrolled successfully!"
                : "You are already enrolled in this course.";

            return RedirectToAction("Available", "Courses");
        }
    }
}