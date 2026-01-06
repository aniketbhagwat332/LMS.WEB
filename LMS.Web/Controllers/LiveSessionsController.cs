using LMS.Services.DTO;
using LMS.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Web.Controllers
{
    [Authorize(Roles = "Instructor")]
    public class LiveSessionsController : Controller
    {
        private readonly LiveSessionService _liveSessionService;

        public LiveSessionsController(LiveSessionService liveSessionService)
        {
            _liveSessionService = liveSessionService;
        }

        // GET: Schedule form
        //public IActionResult Create(int courseId)
        //{
        //    ViewBag.CourseId = courseId;
        ////    return View();
        //}
        public IActionResult Create(int courseId)
        {
            var DTO = new CreateLiveSessionDTO
            {
                CourseId = courseId
            };

            return View(DTO);
        }

        //// POST: Schedule live lecture
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(
        //    int courseId,
        //    string title,
        //    DateTime startTime,
        //    int durationMinutes)
        //{
        //    int instructorId = int.Parse(User.FindFirst("UserId")!.Value);

        //    var success = await _liveSessionService
        //        .ScheduleLiveSessionAsync(
        //            courseId,
        //            instructorId,
        //            title,
        //            startTime,
        //            durationMinutes);

        //    if (!success)
        //        return Forbid(); // ownership violation

        //    return RedirectToAction("Index", new { courseId });
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateLiveSessionDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            int instructorId = int.Parse(User.FindFirst("UserId")!.Value);

            await _liveSessionService.ScheduleLiveSessionAsync(
                model,
                instructorId
            );

            return RedirectToAction(
                "Index",
                new { courseId = model.CourseId }
            );
        }

        // 📅 LIST LIVE SESSIONS PER COURSE
        public async Task<IActionResult> Index(int courseId)
        {
            var sessions = await _liveSessionService
                .GetSessionsByCourseAsync(courseId);

            ViewBag.CourseId = courseId; // needed for "Schedule" button
            return View(sessions);
        }

        

    }
}