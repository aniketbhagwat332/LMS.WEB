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
    public class ProgressRepository : Repository<Progress>, IProgressRepository
    {
        public ProgressRepository(LMSDbContext context) : base(context) { }

        public async Task<bool> IsLessonCompletedAsync(int enrollmentId, int lessonId)
        {
            return await _context.Progresses
                .AnyAsync(p => p.EnrollmentId == enrollmentId
                            && p.LessonId == lessonId
                            && p.IsCompleted);
        }

        public async Task<List<Progress>> GetProgressByEnrollmentAsync(int enrollmentId)
        {
            return await _context.Progresses.AsNoTracking()
                .Where(p => p.EnrollmentId == enrollmentId)
                .ToListAsync();
        }
        public async Task<int> GetCompletedLessonCountAsync(int enrollmentId)
        {
            return await _context.Progresses
                .CountAsync(p => p.EnrollmentId == enrollmentId && p.IsCompleted);
        }
        public async Task<List<int>> GetCompletedLessonIdsAsync(int enrollmentId)
        {
            return await _context.Progresses.AsNoTracking()
                .Where(p => p.EnrollmentId == enrollmentId && p.IsCompleted)
                .Select(p => p.LessonId)
                .ToListAsync();
        }

    }
}