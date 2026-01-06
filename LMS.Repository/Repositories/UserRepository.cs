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
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(LMSDbContext context) : base(context) { }

        public async Task<List<User>> GetInstructorsAsync()
        {
            return await _context.Users.AsNoTracking()
                .Where(u => u.Role == "Instructor")
                .ToListAsync();
        }
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.AsNoTracking().ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public Task<int> CountByRoleAsync(string role)
        {
            return  _context.Users.CountAsync(u => u.Role == role);
        }

        public async Task ToggleUserStatusAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return;

            user.IsActive = !user.IsActive;
            await _context.SaveChangesAsync();
        }

        public async Task<List<User>> GetPendingUsersAsync()
        {
            return await _context.Users.AsNoTracking()
                .Where(u => !u.IsActive)
                .ToListAsync();
        }
        public async Task ActivateUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.IsActive = true;
            }
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);
        }

    }
}
