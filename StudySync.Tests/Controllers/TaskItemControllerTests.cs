using StudySync.Controllers;
using StudySync.DTOs;
using StudySync.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Microsoft.AspNetCore.Mvc;


namespace StudySync.Tests.TaskControllers
{
    public class TaskItemControllerTests
    {
        private readonly Mock<ITaskItemService> _mockService;
        private readonly TaskItemController _controller;

        public TaskItemControllerTests()
        {
            //setup mocks 
            _mockService = new Mock<ITaskItemService>();

            //create controller with mocked dependencies
            _controller= new TaskItemController(_mockService.Object);
        }

        [Fact] // Mark it as test 
        public async Task GetAllTasks_ReturnsOkResult_WithListOfTasks()
        {
            //Arrange: SetUp All dependencies
            var expectedTasks = new List<TaskItemDTO>
            {
                new TaskItemDTO{Id=1, Title="Task 1", Description="Description 1", Priority="High"},
                new TaskItemDTO{Id=2, Title="Task 2", Description="Description 2", Priority="High"}
            };

            //the mocq service should be returning these 2 rows in db 
            _mockService.Setup(service=>service.GetAllTasksAsync()).ReturnsAsync(expectedTasks);

            //Act
            var result = await _controller.GetTasks();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedTask = Assert.IsAssignableFrom<IEnumerable<TaskItemDTO>>(okResult.Value);
            Assert.Equal(2,((List<TaskItemDTO>)returnedTask).Count);

        }
        [Fact]
        public async Task GetTasksByPriority_WithValidPriority_ReturnsOkResultWithMatchingTasks()
        {
            // Arrange
            string priority = "High";
            var expectedTasks = new List<TaskItemDTO>
        {
            new TaskItemDTO { Id = 1, Title = "Task 1", Description = "Description 1", Priority = "High" },
            new TaskItemDTO { Id = 3, Title = "Task 3", Description = "Description 3", Priority = "High" }
        };

            _mockService.Setup(service => service.GetTaksByPriorityAsync(priority))
                .ReturnsAsync(expectedTasks);

            // Act
            var result = await _controller.GetTasksByPriority(priority);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedTasks = Assert.IsAssignableFrom<IEnumerable<TaskItemDTO>>(okResult.Value);
            Assert.Equal(2, ((List<TaskItemDTO>)returnedTasks).Count);
            Assert.All(((List<TaskItemDTO>)returnedTasks), task => Assert.Equal(priority, task.Priority));
        }

        [Fact]
        public async Task GetTasksByPriority_WithEmptyPriority_ReturnsBadRequest()
        {
            // Arrange
            string priority = "";

            // Act
            var result = await _controller.GetTasksByPriority(priority);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Priority value is required", badRequestResult.Value);
        }

        [Fact]
        public async Task GetTasksByPriority_WithInvalidPriority_ReturnsBadRequest()
        {
            // Arrange
            string invalidPriority = "InvalidPriority";
            string errorMessage = "Invalid priority value";

            _mockService.Setup(service => service.GetTaksByPriorityAsync(invalidPriority))
                .ThrowsAsync(new ArgumentException(errorMessage));

            // Act
            var result = await _controller.GetTasksByPriority(invalidPriority);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(errorMessage, badRequestResult.Value);
        }


    }
}
