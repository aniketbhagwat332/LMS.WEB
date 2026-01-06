using LMS.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Repository.Interfaces
{
    public interface ICourseRepository : IRepository<Course>
    {
        Task<List<Course>> GetCoursesWithInstructorAsync();
        Task<List<Course>> GetCoursesByInstructorAsync(int instructorId);
        Task<List<Course>> GetPendingCoursesAsync();
        Task UpdateCourseStatusAsync(int courseId, string status);
        Task<List<Course>> GetApprovedCoursesAsync();
        Task<int> CountByStatusAsync(string status);


    }
}
