using LMS.Repository.Interfaces;
using LMS.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Services.Services
{
    public class ProgressService
    {
        private readonly IProgressRepository _progressRepo;

        public ProgressService(IProgressRepository progressRepo)
        {
            _progressRepo = progressRepo;
        }

        public async Task MarkLessonCompletedAsync(int enrollmentId, int lessonId)
        {
            if (await _progressRepo.IsLessonCompletedAsync(enrollmentId, lessonId))
                return;

            var progress = new Progress
            {
                EnrollmentId = enrollmentId,
                LessonId = lessonId,
                IsCompleted = true
            };

            await _progressRepo.AddAsync(progress);
            await _progressRepo.SaveAsync();
        }
        public async Task<List<int>> GetCompletedLessonsAsync(int enrollmentId)
        {
            return await _progressRepo.GetCompletedLessonIdsAsync(enrollmentId);
        }

    }
}