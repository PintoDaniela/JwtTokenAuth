using JwtTokenTest2.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace JwtTokenTest2.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => e.UserId);
            });
        }
    }
}
