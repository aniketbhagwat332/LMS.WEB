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
    public class EnrollmentRepository : Repository<Enrollment>, IEnrollmentRepository
    {
        public EnrollmentRepository(LMSDbContext context) : base(context) { }

        public async Task<bool> IsStudentEnrolledAsync(int studentId, int courseId)
        {
            return await _context.Enrollments
                .AnyAsync(e => e.StudentId == studentId && e.CourseId == courseId);
        }
        public async Task<List<Enrollment>> GetEnrollmentsByStudentAsync(int studentId)
        {
            return await _context.Enrollments.AsNoTracking()
                .Include(e => e.Course)
                    .ThenInclude(c => c.Instructor)
                .Where(e => e.StudentId == studentId)
                .ToListAsync();
        }

    }
}
