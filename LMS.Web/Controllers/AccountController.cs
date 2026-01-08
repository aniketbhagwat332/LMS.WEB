using LMS.Repository.Models;
using LMS.Services.DTO;
using LMS.Services.Services;
using LMS.Services.Services.Enums;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMS.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly AuthService _authService;

        public AccountController(AuthService authService)
        {
            _authService = authService;
        }
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            await HttpContext.SignOutAsync(); // 🔥 clears auth cookie
            return View();
        }
        

        //[HttpPost]
        //public async Task<IActionResult> Login(string email, string password)
        //{
        //    var user = await _authService.ValidateUserAsync(email, password);

        //    if (user == null)
        //    {
        //        ViewBag.Error = "Invalid credentials";
        //        return View();
        //    }

        //    var claims = new List<Claim>
        //{
        //    new Claim(ClaimTypes.Name, user.FullName),
        //    new Claim(ClaimTypes.Email, user.Email),
        //    new Claim(ClaimTypes.Role, user.Role),
        //    new Claim("UserId", user.UserId.ToString())
        //};

        //    var identity = new ClaimsIdentity(
        //        claims, CookieAuthenticationDefaults.AuthenticationScheme);

        //    await HttpContext.SignInAsync(
        //        CookieAuthenticationDefaults.AuthenticationScheme,
        //        new ClaimsPrincipal(identity));

        //    return user.Role switch
        //    {
        //        "Student" => RedirectToAction("Dashboard", "Student"),
        //        "Instructor" => RedirectToAction("Dashboard", "Instructor"),
        //        "Admin" => RedirectToAction("Dashboard", "Admin"),
        //        _ => RedirectToAction("Login")
        //    };
        //}

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var result = await _authService.ValidateUserAsync(email, password);

            if (result.Result == LoginResult.InvalidCredentials)
            {
                ViewBag.Error = "Invalid email or password";
                return View();
            }

            if (result.Result == LoginResult.PendingApproval)
            {
                ViewBag.Warning = "Your account is pending admin approval.";
               
                    return RedirectToAction(nameof(PendingApproval));

            }

            // ✅ SUCCESS
            var user = result.User!;

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.FullName),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Role, user.Role),
        new Claim("UserId", user.UserId.ToString())
    };

            var identity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));

            return user.Role switch
            {
                "Student" => RedirectToAction("Dashboard", "Student"),
                "Instructor" => RedirectToAction("Dashboard", "Instructor"),
                "Admin" => RedirectToAction("Dashboard", "Admin"),
                _ => RedirectToAction("Login")
            };
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied() => View();

        public IActionResult PendingApproval()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            if (!ModelState.IsValid)
                return View(user);

            await _authService.RegisterAsync(user);

            return RedirectToAction("PendingApproval");
        }

        [Authorize]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.NewPassword != model.ConfirmPassword)
            {
                ModelState.AddModelError("", "Passwords do not match");
                return View(model);
            }

            int userId = int.Parse(User.FindFirst("UserId").Value);

            bool success = await _authService.ChangePasswordAsync(
                userId,
                model.OldPassword,
                model.NewPassword
            );

            if (!success)
            {
                ModelState.AddModelError("", "Old password is incorrect");
                return View(model);
            }

            TempData["Success"] = "Password changed successfully";
            await HttpContext.SignOutAsync();
            return RedirectToAction("Dashboard");
        }

    }
}