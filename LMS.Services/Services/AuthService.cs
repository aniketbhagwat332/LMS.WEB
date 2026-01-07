using LMS.Repository.Interfaces;
using LMS.Repository.Models;
using LMS.Services.Services.Enums;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Services.Services
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        //    public async Task<User?> ValidateUserAsync(string email, string password)
        //    {
        //        var users = await _userRepository.GetAllAsync();
        //        return users.FirstOrDefault(u =>
        //            u.Email == email && u.Password == password &&
        //u.IsActive);
        //    }

        public async Task<(LoginResult Result, User? User)> ValidateUserAsync(
     string email, string password)
        {
            // 1️⃣ Check user exists
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                return (LoginResult.InvalidCredentials, null);
            }

            // 2️⃣ Verify hashed password
            bool isValid = PasswordHasher.VerifyPassword(password, user.Password);
            if (!isValid)
            {
                return (LoginResult.InvalidCredentials, null);
            }

            // 3️⃣ Check account status
            if (!user.IsActive)
            {
                return (LoginResult.PendingApproval, null);
            }

            // 4️⃣ Success
            return (LoginResult.Success, user);
        }

        public async Task RegisterAsync(User user)
        {
            user.IsActive = false;   // 🔒 enforced rule
            user.Role = user.Role;  // Student / Instructor only
            //user.Role = "Admin";  // Student / Instructor only
            user.Password = PasswordHasher.HashPassword(user.Password);
            //user.Password = PasswordHasher.HashPassword("admin123");

            await _userRepository.AddAsync(user);
            await _userRepository.SaveAsync();
        }

        public async Task<bool> ChangePasswordAsync(
    int userId,
    string oldPassword,
    string newPassword)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return false;

            // 🔐 Verify old password
            bool isOldPasswordValid =
                PasswordHasher.VerifyPassword(oldPassword, user.Password);

            if (!isOldPasswordValid)
                return false;

            // 🔐 Hash new password
            user.Password = PasswordHasher.HashPassword(newPassword);

            await _userRepository.UpdateAsync(user);
            return true;
        }

    }
}
