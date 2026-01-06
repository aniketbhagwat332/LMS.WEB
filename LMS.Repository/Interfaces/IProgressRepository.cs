using LMS.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Repository.Interfaces
{
    public interface IProgressRepository : IRepository<Progress>
    {
        Task<bool> IsLessonCompletedAsync(int enrollmentId, int lessonId);
        Task<List<Progress>> GetProgressByEnrollmentAsync(int enrollmentId);
        Task<int> GetCompletedLessonCountAsync(int enrollmentId);
        Task<List<int>> GetCompletedLessonIdsAsync(int enrollmentId);


    }
}
