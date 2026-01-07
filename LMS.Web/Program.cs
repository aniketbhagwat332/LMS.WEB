using LMS.Repository.Data;
using LMS.Repository.Interfaces;
using LMS.Repository.Repositories;
using LMS.Services.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System;




namespace LMS.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            //      builder.Services.AddDbContext<LMSDbContext>(options =>
            //options.UseSqlServer(
            //    builder.Configuration.GetConnectionString("LMSConnection")));


            builder.Services.AddDbContext<LMSDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("LMSConnection"),
    x => x.MigrationsAssembly("LMS.Repository")));



            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            builder.Services.AddScoped<ICourseRepository, CourseRepository>();
            builder.Services.AddScoped<CourseService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
            builder.Services.AddScoped<EnrollmentService>();
            builder.Services.AddScoped<ILessonRepository, LessonRepository>();
            builder.Services.AddScoped<LessonService>();
            builder.Services.AddScoped<IProgressRepository, ProgressRepository>();
            builder.Services.AddScoped<ProgressService>();
            builder.Services.AddScoped<AdminService>();
            builder.Services.AddScoped<ZoomAuthService>();
            builder.Services.AddScoped<ZoomMeetingService>();
            builder.Services.AddScoped<LiveSessionService>();










            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    });

            builder.Services.AddAuthorization();




            var app = builder.Build();
            
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<LMSDbContext>();
                db.Database.Migrate();   // 👈 THIS creates tables on Azure
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
