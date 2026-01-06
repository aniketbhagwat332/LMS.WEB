using LMS.Repository.Interfaces;
using LMS.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Services.Services
{
    public class LessonService
    {
        private readonly ILessonRepository _lessonRepo;

        public LessonService(ILessonRepository lessonRepo)
        {
            _lessonRepo = lessonRepo;
        }

        public Task<List<Lesson>> GetLessonsByCourseAsync(int courseId)
            => _lessonRepo.GetLessonsByCourseAsync(courseId);

        public async Task AddLessonAsync(Lesson lesson)
        {
            await _lessonRepo.AddAsync(lesson);
            await _lessonRepo.SaveAsync();
        }

        public async Task<Lesson?> GetByIdAsync(int id)
            => await _lessonRepo.GetByIdAsync(id);

        public async Task UpdateLessonAsync(Lesson lesson)
        {
            _lessonRepo.Update(lesson);
            await _lessonRepo.SaveAsync();
        }

        public async Task DeleteLessonAsync(int id)
        {
            var lesson = await _lessonRepo.GetByIdAsync(id);
            if (lesson != null)
            {
                _lessonRepo.Delete(lesson);
                await _lessonRepo.SaveAsync();
            }
        }
    }
}
