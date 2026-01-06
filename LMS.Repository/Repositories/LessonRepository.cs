using LMS.Repository.Data;
using LMS.Repository.Interfaces;
using LMS.Repository.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Repository.Repositories
{
    public class LessonRepository : Repository<Lesson>, ILessonRepository
    {
        public LessonRepository(LMSDbContext context) : base(context) { }

        public async Task<List<Lesson>> GetLessonsByCourseAsync(int courseId)
        {
            return await _context.Lessons.AsNoTracking()
                .Where(l => l.CourseId == courseId)
                .OrderBy(l => l.OrderNo)
                .ToListAsync();
        }
        public async Task<int> GetLessonCountByCourseAsync(int courseId)
        {
            return await _context.Lessons.AsNoTracking()
                .CountAsync(l => l.CourseId == courseId);
        }

    }
}
