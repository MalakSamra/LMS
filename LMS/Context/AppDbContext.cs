using LMS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace LMS.Data
{
    public class AppDbContext : DbContext
    {
        public virtual DbSet<User> users { get; set; }
        public virtual DbSet<Course> courses { get; set; }
        public virtual DbSet<Module> modules { get; set; }
        public virtual DbSet<Progress> progresses { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql("Host=localhost; Database=LMS; Username=postgres; Password=123456");
            base.OnConfiguring(options);
        }
        
    }
}
