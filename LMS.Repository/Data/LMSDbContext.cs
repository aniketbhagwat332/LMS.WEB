using LMS.Repository.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Repository.Data
{
    public class LMSDbContext : DbContext
    {
        public LMSDbContext(DbContextOptions<LMSDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Enrollment → Student (NO CASCADE)
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Student)
                .WithMany()
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Progress → Enrollment (CASCADE OK)
            modelBuilder.Entity<Progress>()
                .HasOne(p => p.Enrollment)
                .WithMany()
                .HasForeignKey(p => p.EnrollmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // ❌ IMPORTANT FIX: Progress → Lesson (NO CASCADE)
            modelBuilder.Entity<Progress>()
                .HasOne(p => p.Lesson)
                .WithMany()
                .HasForeignKey(p => p.LessonId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LiveSession>()
       .HasOne(ls => ls.Instructor)
       .WithMany()
       .HasForeignKey(ls => ls.InstructorId)
       .OnDelete(DeleteBehavior.Restrict); // 🚫 NO CASCADE
        }


        public DbSet<User> Users { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Progress> Progresses { get; set; }
        public DbSet<LiveSession> LiveSessions { get; set; }


    }
}
