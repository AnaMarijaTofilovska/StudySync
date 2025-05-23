using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudySync.Models;

namespace StudySync.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { } //Opening up connection to DB


        //With DBSet we say that we want to create tables which is mapped in <Table> (model)
        public DbSet<User> Users { get; set; }
        public DbSet<Taskitem> Tasks { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Subtask> Subtasks { get; set; }
        public DbSet<Reminder> Reminders { get; set; }

        // NEXT: Dependency Injection : For our app to work we want controller model data,

        //NEXT: AFTER CONTROLLER INSERT DATA,SEED DB
        //when model is created for these entities make sure data is there 
        //NEXT migration

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1) Seed Users
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Name = "Alice", Email = "alice@example.com" },
                new User { Id = 2, Name = "Bob", Email = "bob@example.com" }
            );

            // 2) Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Homework" },
                new Category { Id = 2, Name = "Exams" }
            );

            // 3) Seed Tasks (reference seeded Users, Categories)
            modelBuilder.Entity<Taskitem>().HasData(
                new Taskitem { Id = 1, Title = "Math Assignment", UserId = 1, CategoryId = 1, Description = "Test", Priority = "low" },
                new Taskitem { Id = 2, Title = "Study for Physics", UserId = 2, CategoryId = 2, Description = "Test New", Priority = "high" }
            );

        }

    }
}
