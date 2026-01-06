using LMS.Repository.Models;
using LMS.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Web.Controllers
{
    [Authorize(Roles = "Instructor,Admin")]
    public class LessonsController : Controller
    {
        private readonly LessonService _lessonService;

        public LessonsController(LessonService lessonService)
        {
            _lessonService = lessonService;
        }

        // GET: Lessons?courseId=1
        public async Task<IActionResult> Index(int courseId)
        {
            ViewBag.CourseId = courseId;
            var lessons = await _lessonService.GetLessonsByCourseAsync(courseId);
            return View(lessons);
        }

        public IActionResult Create(int courseId)
        {
            return View(new Lesson { CourseId = courseId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Lesson lesson)
        {
            if (!ModelState.IsValid) return View(lesson);

            await _lessonService.AddLessonAsync(lesson);
            return RedirectToAction(nameof(Index), new { courseId = lesson.CourseId });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var lesson = await _lessonService.GetByIdAsync(id);
            if (lesson == null) return NotFound();
            return View(lesson);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Lesson lesson)
        {
            if (!ModelState.IsValid) return View(lesson);

            await _lessonService.UpdateLessonAsync(lesson);
            return RedirectToAction(nameof(Index), new { courseId = lesson.CourseId });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var lesson = await _lessonService.GetByIdAsync(id);
            if (lesson == null) return NotFound();
            return View(lesson);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lesson = await _lessonService.GetByIdAsync(id);
            if (lesson == null) return NotFound();

            int courseId = lesson.CourseId;
            await _lessonService.DeleteLessonAsync(id);
            return RedirectToAction(nameof(Index), new { courseId });
        }
    }
}