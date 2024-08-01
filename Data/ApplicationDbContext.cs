using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectMenager.Models;

namespace ProjectMenager.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Project>? Projects { get; set; }
        public DbSet<Member>? Member { get; set; }
        public DbSet<Item>? Item { get; set; }
        public DbSet<Models.Task>? Task { get; set; }
        public DbSet<Notes>? Notes { get; set; }

        public DbSet<IdentityUser>? Employee { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}