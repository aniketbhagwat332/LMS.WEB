using LMS.Repository.Models;

namespace LMS.Repository.Interfaces
{
    public interface ILessonRepository : IRepository<Lesson>
    {
        Task<List<Lesson>> GetLessonsByCourseAsync(int courseId);
        Task<int> GetLessonCountByCourseAsync(int courseId);

    }
}
