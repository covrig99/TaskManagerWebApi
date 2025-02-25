using Microsoft.EntityFrameworkCore;
using TaskManagerWebApi.Models;

namespace TaskManagerWebApi.DataAccessLayer
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Tasks> Tasks { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
