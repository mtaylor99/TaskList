using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using TaskList.DAL.Interfaces;
using TaskList.DAL.Models;
using TaskList.WebApi.Controllers;

namespace TaskList.Tests.BusinessSetup
{
    [TestClass]
    public class WebApiControllerTests
    {
        [TestMethod]
        public void WebApi_Tasks_Get()
        {
            // Arrange
            Mock<ILogger<TasksController>> mockLogger = new Mock<ILogger<TasksController>>();
            Mock<ITaskListRepository> mockTaskListRepository = new Mock<ITaskListRepository>();

            mockTaskListRepository.Setup(m => m.GetTasks).Returns((new Task[] {
                new Task { TaskId = 1, Description = "T1", StatusId = 1, LocationId = 1, UserId = "1", Location = new Location { Description = "L1" }, Status = new Status { Name = "Open" }, User = new AspNetUser { FullName = "Joe" } },
                new Task { TaskId = 1, Description = "T2", StatusId = 1, LocationId = 1, UserId = "1", Location = new Location { Description = "L2" }, Status = new Status { Name = "Open" }, User = new AspNetUser { FullName = "Joe" } },
                new Task { TaskId = 1, Description = "T3", StatusId = 1, LocationId = 1, UserId = "1", Location = new Location { Description = "L3" }, Status = new Status { Name = "Open" }, User = new AspNetUser { FullName = "Joe" } },
                new Task { TaskId = 1, Description = "T4", StatusId = 1, LocationId = 1, UserId = "1", Location = new Location { Description = "L4" }, Status = new Status { Name = "Open" }, User = new AspNetUser { FullName = "Joe" } },
                new Task { TaskId = 1, Description = "T5", StatusId = 1, LocationId = 1, UserId = "1", Location = new Location { Description = "L5" }, Status = new Status { Name = "Open" }, User = new AspNetUser { FullName = "Joe" } }
            }).AsQueryable<Task>());

            TasksController controller = new TasksController(mockLogger.Object, mockTaskListRepository.Object);

            // Act
            OkObjectResult result = controller.Get(string.Empty, string.Empty, "1", "9999") as OkObjectResult;

            var tasks = JsonConvert.DeserializeObject<List<Task>>(result.Value.ToString());

            // Assert
            Assert.AreEqual("T5", tasks[0].Description);
            Assert.AreEqual("T4", tasks[1].Description);
            Assert.AreEqual("T3", tasks[2].Description);
            Assert.AreEqual("T2", tasks[3].Description);
            Assert.AreEqual("T1", tasks[4].Description);
        }

        [TestMethod]
        public void WebApi_Locations_Get()
        {
            // Arrange
            Mock<ILogger<LocationsController>> mockLogger = new Mock<ILogger<LocationsController>>();
            Mock<ILocationRepository> mockLocationRepository = new Mock<ILocationRepository>();

            mockLocationRepository.Setup(m => m.GetLocations).Returns((new Location[] {
                new Location { LocationId = 1, ParentId = 0, Description = "L1" },
                new Location { LocationId = 2, ParentId = 1, Description = "L2" },
                new Location { LocationId = 3, ParentId = 1, Description = "L3" },
                new Location { LocationId = 4, ParentId = 1, Description = "L4" },
                new Location { LocationId = 5, ParentId = 1, Description = "L5" }
            }).AsQueryable<Location>());

            LocationsController controller = new LocationsController(mockLogger.Object, mockLocationRepository.Object);

            // Act
            OkObjectResult result = controller.Get("1", "9999") as OkObjectResult;

            var locations = JsonConvert.DeserializeObject<List<Location>>(result.Value.ToString());

            // Assert
            Assert.AreEqual("L1", locations[0].Description);
            Assert.AreEqual("L2", locations[1].Description);
            Assert.AreEqual("L3", locations[2].Description);
            Assert.AreEqual("L4", locations[3].Description);
            Assert.AreEqual("L5", locations[4].Description);
        }
    }
}
