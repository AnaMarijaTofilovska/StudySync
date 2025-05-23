using AutoMapper;
using StudySync.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using StudySync.Services;
using StudySync.Models;
using StudySync.DTOs;

namespace StudySync.Tests.Services
{
    public class TaskItemServiceTests
    {
        //WE NEED TO MOQ EVERYTHING WE USE IN THE REAL TASK ITEM SERVICE , so for my services to work we need 3 depe
        private readonly Mock<ITaskItemRepository> _mockTaskRepository;
        private readonly Mock<IUserReposiotry> _mockUserRepository;
        private readonly Mock<IMapper> _mockMapper;
        //This is not moq
        private readonly TaskItemService _taskItemService;

        public TaskItemServiceTests()
        {
            _mockTaskRepository = new Mock<ITaskItemRepository>();
            _mockUserRepository = new Mock<IUserReposiotry>();
            _mockMapper = new Mock<IMapper>();

            // create service with the dependencies mocked
            _taskItemService = new TaskItemService(
                _mockTaskRepository.Object,
                _mockUserRepository.Object,
                _mockMapper.Object
            );
        }

        [Fact]
        public async Task GetAllTasksAsync_ReturnsAllTasks()
        {
            //Arrange 
            var taskItems = new List<Taskitem>
            {
                new Taskitem 
                {
                    Id=1,
                    Title="Task 1",
                    Description="Description 1",
                    Priority="High"
                },
                new Taskitem
                {
                    Id=2,
                    Title="Task 2",
                    Description="Description 2",
                    Priority="Medium"
                }
            };

            var taskitemDTOs = new List<TaskItemDTO>
            {
                new TaskItemDTO
                {
                    Id=1,
                    Title="Task 1",
                    Description="Description 1",
                    Priority="High"
                },
                new TaskItemDTO
                {
                    Id=2,
                    Title="Task 2",
                    Description="Description 2",
                    Priority="Medium"
                }
            };

            //Setup repository mock to return test data 
            _mockTaskRepository.Setup(repo=>repo.GetAllTasksAsync()).ReturnsAsync(taskItems);

            //Setup mapper mock to map from domain models to dtos
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<TaskItemDTO>>(taskItems));

            //Act
            var result=await _taskItemService.GetAllTasksAsync();

            //Assert 
            //we dont need for each column test, we can just the first 2 ones 
            var resultList=result.ToList();
            Assert.Equal(2, resultList.Count);

            //Assert values are correct for first row
            Assert.Equal(1, resultList[0].Id);
            Assert.Equal("Task 1", resultList[0].Title);

            //Assert values are correct for second row/item
            Assert.Equal(2, resultList[1].Id);
            Assert.Equal("Task 2", resultList[1].Title);

            // Verify repository was called
            _mockTaskRepository.Verify(repo => repo.GetAllTasksAsync(), Times.Once);

            // Verify mapper was called with the correct parameters
            _mockMapper.Verify(mapper => mapper.Map<IEnumerable<TaskItemDTO>>(taskItems), Times.Once);
        }

        //What if the repo returns an empty list?
        [Fact]
        public async Task GetAllTasksAsync_EmptyList_ReturnEmptyList()
        {
            var taskitems = new List<Taskitem>();
            var taskitemDtos = new List<TaskItemDTO>();
            _mockTaskRepository.Setup(r => r.GetAllTasksAsync()).ReturnsAsync(taskitems);
            _mockMapper.Setup(m => m.Map<IEnumerable<TaskItemDTO>>(taskitems));

            var result = await _taskItemService.GetAllTasksAsync();


            Assert.NotNull(result);
            Assert.Empty(result);

        }

        [Fact]
        public async Task GetTasksByPriorityAsync_ValidPriority_ReturnsOnlyMatchingTasks()
        {
            // Arrange
            var priority = "High";

            var allTasks = new List<Taskitem>
    {
        new Taskitem { Id = 1, Title = "Task 1", Priority = "High" },
        new Taskitem { Id = 2, Title = "Task 2", Priority = "Low" }
    };

            var expectedDTOs = new List<TaskItemDTO>
    {
        new TaskItemDTO { Id = 1, Title = "Task 1", Priority = "High" }
    };

            // Set up the repository to return all tasks
            _mockTaskRepository.Setup(repo => repo.GetAllTasksAsync()).ReturnsAsync(allTasks);

            // Set up the mapper to return only the DTOs with matching priority
            _mockMapper.Setup(m => m.Map<IEnumerable<TaskItemDTO>>(It.IsAny<IEnumerable<Taskitem>>()))
                       .Returns((IEnumerable<Taskitem> tasks) =>
                           tasks.Where(t => t.Priority == priority)
                                .Select(t => new TaskItemDTO { Id = t.Id, Title = t.Title, Priority = t.Priority }));

            // Act
            var result = await _taskItemService.GetTaksByPriorityAsync(priority);

            // Assert
            Assert.Single(result);
            Assert.Equal("High", result.First().Priority);
        }

    }
}
