using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BKBCollegeManagement.Models
{
    
    public class UserContext : DbContext
        {
        public UserContext(DbContextOptions<UserContext> options)
           : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }

        public DbSet<Course> Courses { get; set; } = null!;

        public DbSet<Student> Students { get; set; } = null!;

        public DbSet<Subject> Subjects { get; set; } = null!;

        public DbSet<Announcement> Announcements { get; set; } = null!;

        //   protected override void OnModelCreating(ModelBuilder modelBuilder)
        //   {
        //     modelBuilder.Entity<Message>().HasKey(m => new { m.SenderName });

        // }
    }

}
