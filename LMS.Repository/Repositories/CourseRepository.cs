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
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        public CourseRepository(LMSDbContext context) : base(context)
        {
        }

        public async Task<List<Course>> GetCoursesWithInstructorAsync()
        {
            return await _context.Courses.AsNoTracking()
                .Include(c => c.Instructor)
                .ToListAsync();
        }
        public async Task<List<Course>> GetCoursesByInstructorAsync(int instructorId)
        {
            return await _context.Courses.AsNoTracking()
                .Include(c => c.Lessons)
                .Include(c => c.Enrollments)
                .Where(c => c.InstructorId == instructorId)
                .ToListAsync();
        }


        public async Task<List<Course>> GetPendingCoursesAsync()
        {
            return await _context.Courses.AsNoTracking()
                .Include(c => c.Instructor)
                .Where(c => c.Status == "Pending")
                .ToListAsync();
        }

        public async Task UpdateCourseStatusAsync(int courseId, string status)
        {
            var course = await _context.Courses.FindAsync(courseId);
            if (course == null) return;

            course.Status = status;
            await _context.SaveChangesAsync();
        }

        public async Task<List<Course>> GetApprovedCoursesAsync()
        {
            return await _context.Courses
                  .Where(c => c.Status == "Approved")
                  .Include(c => c.Instructor)
                  .ToListAsync();
        }
        public async Task<int> CountByStatusAsync(string status)
        {
            return await _context.Courses
                .CountAsync(c => c.Status == status);
        }

    }

}
