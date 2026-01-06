using LMS.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Repository.Interfaces
{
    public interface IEnrollmentRepository : IRepository<Enrollment>
    {
        Task<bool> IsStudentEnrolledAsync(int studentId, int courseId);

        Task<List<Enrollment>> GetEnrollmentsByStudentAsync(int studentId);
    }
}
