using Microsoft.EntityFrameworkCore;
using StudySync.Data;
using StudySync.Models;
using StudySync.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace StudySync.Tests.Repositories
{
    public class TaskItemRepositoryTest : IDisposable
    {
        private readonly string _databaseName;
        private readonly DbContextOptions<ApplicationDbContext> _options;

        public TaskItemRepositoryTest()
        {
            // Create a unique database name
            _databaseName = $"TestTaskDatabase_{Guid.NewGuid()}";

            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: _databaseName)
                .Options;

            // Clean start - create and seed the database
            using (var context = new ApplicationDbContext(_options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                SeedDatabase(context);
            }
        }

        // IDisposable implementation to clean up after tests
        public void Dispose()
        {
            // Clean up after test
            using (var context = new ApplicationDbContext(_options))
            {
                context.Database.EnsureDeleted();
            }
        }

        private void SeedDatabase(ApplicationDbContext context)
        {
            // First ensure the database is empty
            context.Tasks.RemoveRange(context.Tasks);
            context.SaveChanges();

            // Add test data
            context.Tasks.AddRange(
                new Taskitem
                {
                    Title = "Task 1",
                    Description = "Description 1",
                    Priority = "High",
                    isCompleted = false
                },
                new Taskitem
                {
                    Title = "Task 2",
                    Description = "Description 2",
                    Priority = "Medium",
                    isCompleted = true
                }
            );

            context.SaveChanges();
        }

        [Fact]
        public async Task GetAllTasksAsync_ReturnsAllTasks()
        {
            // Arrange - create a new context for the test
            using var context = new ApplicationDbContext(_options);
            var repository = new TaskItemRepository(context);

            // Verify database state before test
            var count = await context.Tasks.CountAsync();
            Assert.Equal(2, count); // Verify we have exactly 2 tasks before proceeding

            // Act
            var result = await repository.GetAllTasksAsync();

            // Assert
            var tasks = result.ToList();
            Assert.Equal(2, tasks.Count);

            // Find tasks by title
            var task1 = tasks.FirstOrDefault(t => t.Title == "Task 1");
            var task2 = tasks.FirstOrDefault(t => t.Title == "Task 2");

            Assert.NotNull(task1);
            Assert.NotNull(task2);
            Assert.Equal("High", task1.Priority);
            Assert.Equal("Medium", task2.Priority);
            Assert.False(task1.isCompleted);
            Assert.True(task2.isCompleted);
        }

    }
}
