using LMS.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Web.Controllers
{
    [Authorize(Roles = "Student")]
    public class ProgressController : Controller
    {
        private readonly ProgressService _progressService;

        public ProgressController(ProgressService progressService)
        {
            _progressService = progressService;
        }

        [HttpPost]
        public async Task<IActionResult> Complete(int enrollmentId, int lessonId)
        {
            await _progressService.MarkLessonCompletedAsync(enrollmentId, lessonId);
            return RedirectToAction("CourseLessons", "Student",
                new { enrollmentId });
        }
    }
}
