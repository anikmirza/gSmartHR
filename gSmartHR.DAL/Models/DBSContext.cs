using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace gSmartHR.DAL.Models
{
    public class DBSContext : DbContext
    {
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Attendance> Attendances { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>()
                .Property(b => b.Id)
                .IsRequired();
            modelBuilder.Entity<ApplicationUser>()
                .Property(b => b.Id)
                .IsRequired();
            modelBuilder.Entity<Employee>()
                .Property(b => b.Id)
                .IsRequired();
            modelBuilder.Entity<Employee>()
                .HasOne(b => b.ApplicationUser)
                .WithOne(i => i.Employee)
                .HasForeignKey<ApplicationUser>(b => b.EmployeeId);
            modelBuilder.Entity<Attendance>()
                .Property(b => b.Id)
                .IsRequired();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string TempStr = System.IO.File.ReadAllText("DBConnectionString.txt");
            optionsBuilder.UseSqlServer(TempStr);
        }
    }
}
