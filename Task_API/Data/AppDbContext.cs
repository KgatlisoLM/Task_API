using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Task_API.Models;

namespace Task_API.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<TaskDo> Tasks { get; set; }
        public DbSet<Status> Statuses { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Status>().HasData(
                  
             new Status
             {
                 Id = 1,
                 Name = "To Do",
             },
             new Status
             {
                Id = 2,
                Name = "In Progress",
             },
             new Status
             {
                Id = 3,
                Name = "Done",
             });

            builder.Entity<TaskDo>().HasData(

             new TaskDo
              {
                    Id = 1,
                    Title = "Task to do test",
                    Description = "description of the task to do",
                    CreatedAt = DateTime.Now,
                    StatusId = 1,
             },
             new TaskDo
             {
                    Id = 2,
                    Title = "Task in progress test",
                    Description = "description of the task in progress",
                    CreatedAt = DateTime.Now,
                    StatusId = 2,
             },
             new TaskDo
             {
                   Id = 3,
                   Title = "Task done test",
                   Description = "description of the task done",
                   CreatedAt = DateTime.Now,
                   StatusId = 3,
             });

        }
    }
}
