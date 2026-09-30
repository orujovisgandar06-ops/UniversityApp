using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace UniversityApp
{
    public class AppDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; }

        public DbSet<StudentCard> StudentCards { get; set; }

        public DbSet<Teacher> Teachers { get; set; }

        public DbSet<Course> Courses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=UniversityApp;User Id=admin;Password=admin;TrustServerCertificate=True;");
        }
    }
}