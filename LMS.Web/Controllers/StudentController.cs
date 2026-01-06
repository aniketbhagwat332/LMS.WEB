using LMS.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Web.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        private readonly EnrollmentService _enrollmentService;
        private readonly LessonService _lessonService;
        private readonly ProgressService _progressService;


        public StudentController(EnrollmentService enrollmentService,LessonService lessonService, ProgressService progressService)
        {
            _enrollmentService = enrollmentService;
            _lessonService = lessonService;
            _progressService = progressService;
        }

        public async Task<IActionResult> Dashboard()
        {
            int studentId = int.Parse(User.FindFirst("UserId")!.Value);

            var enrollments = await _enrollmentService
                .GetStudentEnrollmentsAsync(studentId);

            var progressData = new List<dynamic>();

            foreach (var e in enrollments)
            {
                int percentage =
                    await _enrollmentService.GetCourseProgressPercentageAsync(
                        e.EnrollmentId, e.CourseId);

                progressData.Add(new
                {
                    Enrollment = e,
                    Percentage = percentage
                });
            }

            return View(progressData);
        }


        public async Task<IActionResult> CourseLessons(int enrollmentId)
        {
            int studentId = int.Parse(User.FindFirst("UserId")!.Value);

            var enrollment = (await _enrollmentService
                .GetStudentEnrollmentsAsync(studentId))
                .First(e => e.EnrollmentId == enrollmentId);

            var lessons = await _lessonService
                .GetLessonsByCourseAsync(enrollment.CourseId);

            var completedLessonIds =
                await _progressService.GetCompletedLessonsAsync(enrollmentId);

            ViewBag.CompletedLessonIds = completedLessonIds;
            ViewBag.EnrollmentId = enrollmentId;

            return View(lessons);
        }


    }
}