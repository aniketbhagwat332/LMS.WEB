using LMS.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Repository.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<List<User>> GetInstructorsAsync();
        Task<List<User>> GetAllUsersAsync();
        Task<User?> GetByIdAsync(int userId);
        Task UpdateAsync(User user);

        Task<int> CountByRoleAsync(string role);

        Task ToggleUserStatusAsync(int userId);

        Task<List<User>> GetPendingUsersAsync();
        Task ActivateUserAsync(int userId);
        Task<User?> GetByEmailAsync(string email);




    }
}
