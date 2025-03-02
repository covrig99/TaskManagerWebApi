using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity;
using TaskManagerWebApi.Models;

namespace TaskManagerWebApi.DataAccessLayer
{
    public class TaskManageDbContext : IdentityDbContext<User, IdentityRole<int>, int,
        IdentityUserClaim<int>, IdentityUserRole<int>, IdentityUserLogin<int>,
        IdentityRoleClaim<int>, IdentityUserToken<int>>

    {

        public TaskManageDbContext(DbContextOptions <TaskManageDbContext>options) : base(options)
        {

        }


        public DbSet<User> Users { get; set; }
        public DbSet<UserTask> Tasks { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)

        {
            modelBuilder.Entity<IdentityUserLogin<int>>().HasKey(x => new { x.UserId, x.LoginProvider, x.ProviderKey });
            modelBuilder.Entity<IdentityUserRole<int>>().HasKey(x => new { x.UserId, x.RoleId });
            modelBuilder.Entity<IdentityUserClaim<int>>().HasKey(x => x.Id);
            modelBuilder.Entity<IdentityRoleClaim<int>>().HasKey(x => x.Id);
            modelBuilder.Entity<IdentityUserToken<int>>().HasKey(x => new { x.UserId, x.LoginProvider, x.Name });
            modelBuilder.Entity<UserTask>()
                .HasOne<User>()
                .WithMany(u => u.AssignedTasks)
                .HasForeignKey(t => t.IdUser)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
              .HasIndex(e => e.Email)
              .IsUnique();

        }
    }
}
