using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TaskList.DAL.Interfaces;
using TaskList.DAL.Models;
using TaskList.Web.Areas.Project.Controllers;
using TaskList.Web.Models.ViewModels;

namespace TaskList.Tests.BusinessSetup
{
    [TestClass]
    public class TasksControllerTests
    {
        [TestMethod]
        public void Task_Can_Paginate()
        {
            // Arrange
            Mock<ILogger<TasksController>> mockLogger = new Mock<ILogger<TasksController>>();
            Mock<ITaskListRepository> mockTaskListRepository = new Mock<ITaskListRepository>();
            Mock<IStatusRepository> mockStatusRepository = new Mock<IStatusRepository>();
            Mock<ILocationRepository> mockLocationRepository = new Mock<ILocationRepository>();
            Mock<IIdentityRepository> mockIdentityRepository = new Mock<IIdentityRepository>();

            mockTaskListRepository.Setup(m => m.GetTasks).Returns((new Task[] {
                new Task { TaskId = 1, Description = "T1", StatusId = 1, LocationId = 1, UserId = "1", Location = new Location { Description = "L1" }, Status = new Status { Name = "Open" }, User = new AspNetUser { FullName = "Joe" } },
                new Task { TaskId = 1, Description = "T2", StatusId = 1, LocationId = 1, UserId = "1", Location = new Location { Description = "L2" }, Status = new Status { Name = "Open" }, User = new AspNetUser { FullName = "Joe" } },
                new Task { TaskId = 1, Description = "T3", StatusId = 1, LocationId = 1, UserId = "1", Location = new Location { Description = "L3" }, Status = new Status { Name = "Open" }, User = new AspNetUser { FullName = "Joe" } },
                new Task { TaskId = 1, Description = "T4", StatusId = 1, LocationId = 1, UserId = "1", Location = new Location { Description = "L4" }, Status = new Status { Name = "Open" }, User = new AspNetUser { FullName = "Joe" } },
                new Task { TaskId = 1, Description = "T5", StatusId = 1, LocationId = 1, UserId = "1", Location = new Location { Description = "L5" }, Status = new Status { Name = "Open" }, User = new AspNetUser { FullName = "Joe" } }
            }).AsQueryable<Task>());

            TasksController controller = new TasksController(mockLogger.Object, mockTaskListRepository.Object, mockStatusRepository.Object, mockLocationRepository.Object, mockIdentityRepository.Object);
            controller.PageSize = 3;

            // Act
            TasksListViewModel result = controller.List(string.Empty, string.Empty, 2).ViewData.Model as TasksListViewModel;

            // Assert
            Task[] TaskArray = result.Tasks.ToArray();
            Assert.IsTrue(TaskArray.Length == 2);
            Assert.AreEqual("T2", TaskArray[0].Description);
            Assert.AreEqual("T1", TaskArray[1].Description);
        }

        [TestMethod]
        public void Task_Can_Send_Pagination()
        {
            // Arrange
            Mock<ILogger<TasksController>> mockLogger = new Mock<ILogger<TasksController>>();
            Mock<ITaskListRepository> mockTaskListRepository = new Mock<ITaskListRepository>();
            Mock<IStatusRepository> mockStatusRepository = new Mock<IStatusRepository>();
            Mock<ILocationRepository> mockLocationRepository = new Mock<ILocationRepository>();
            Mock<IIdentityRepository> mockIdentityRepository = new Mock<IIdentityRepository>();

            mockTaskListRepository.Setup(m => m.GetTasks).Returns((new Task[] {
                new Task { TaskId = 1, Description = "T1", StatusId = 1, LocationId = 1, UserId = "1", Location = new Location { Description = "L1" }, Status = new Status { Name = "Open" }, User = new AspNetUser { FullName = "Joe" } },
                new Task { TaskId = 1, Description = "T2", StatusId = 1, LocationId = 1, UserId = "1", Location = new Location { Description = "L2" }, Status = new Status { Name = "Open" }, User = new AspNetUser { FullName = "Joe" } },
                new Task { TaskId = 1, Description = "T3", StatusId = 1, LocationId = 1, UserId = "1", Location = new Location { Description = "L3" }, Status = new Status { Name = "Open" }, User = new AspNetUser { FullName = "Joe" } },
                new Task { TaskId = 1, Description = "T4", StatusId = 1, LocationId = 1, UserId = "1", Location = new Location { Description = "L4" }, Status = new Status { Name = "Open" }, User = new AspNetUser { FullName = "Joe" } },
                new Task { TaskId = 1, Description = "T5", StatusId = 1, LocationId = 1, UserId = "1", Location = new Location { Description = "L5" }, Status = new Status { Name = "Open" }, User = new AspNetUser { FullName = "Joe" } }
            }).AsQueryable<Task>());

            TasksController controller = new TasksController(mockLogger.Object, mockTaskListRepository.Object, mockStatusRepository.Object, mockLocationRepository.Object, mockIdentityRepository.Object);
            controller.PageSize = 3;

            // Act
            TasksListViewModel result = controller.List(string.Empty, string.Empty, 2).ViewData.Model as TasksListViewModel;

            // Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(2, pageInfo.CurrentPage);
            Assert.AreEqual(3, pageInfo.ItemsPerPage);
            Assert.AreEqual(5, pageInfo.TotalItems);
            Assert.AreEqual(2, pageInfo.TotalPages);
        }

        [TestMethod]
        public void Task_Can_Search()
        {
            // Arrange
            Mock<ILogger<TasksController>> mockLogger = new Mock<ILogger<TasksController>>();
            Mock<ITaskListRepository> mockTaskListRepository = new Mock<ITaskListRepository>();
            Mock<IStatusRepository> mockStatusRepository = new Mock<IStatusRepository>();
            Mock<ILocationRepository> mockLocationRepository = new Mock<ILocationRepository>();
            Mock<IIdentityRepository> mockIdentityRepository = new Mock<IIdentityRepository>();

            mockTaskListRepository.Setup(m => m.GetTasks).Returns((new Task[] {
                new Task { TaskId = 1, Description = "T1", StatusId = 1, LocationId = 1, UserId = "1", Location = new Location { Description = "L1" }, Status = new Status { Name = "Open" }, User = new AspNetUser { FullName = "Joe" } },
                new Task { TaskId = 1, Description = "T2", StatusId = 1, LocationId = 1, UserId = "1", Location = new Location { Description = "L2" }, Status = new Status { Name = "Open" }, User = new AspNetUser { FullName = "Joe" } },
                new Task { TaskId = 1, Description = "T3", StatusId = 1, LocationId = 1, UserId = "1", Location = new Location { Description = "L3" }, Status = new Status { Name = "Open" }, User = new AspNetUser { FullName = "Joe" } },
                new Task { TaskId = 1, Description = "T4", StatusId = 1, LocationId = 1, UserId = "1", Location = new Location { Description = "L4" }, Status = new Status { Name = "Open" }, User = new AspNetUser { FullName = "Joe" } },
                new Task { TaskId = 1, Description = "T5", StatusId = 1, LocationId = 1, UserId = "1", Location = new Location { Description = "L5" }, Status = new Status { Name = "Open" }, User = new AspNetUser { FullName = "Joe" } }
            }).AsQueryable<Task>());

            TasksController controller = new TasksController(mockLogger.Object, mockTaskListRepository.Object, mockStatusRepository.Object, mockLocationRepository.Object, mockIdentityRepository.Object);
            controller.PageSize = 3;

            // Act
            TasksListViewModel result = controller.List("T1", string.Empty, 1).ViewData.Model as TasksListViewModel;

            // Assert
            Task[] TaskArray = result.Tasks.ToArray();
            Assert.IsTrue(TaskArray.Length == 1);
            Assert.AreEqual("T1", TaskArray[0].Description);
        }

        [TestMethod]
        public void Task_Can_Sort()
        {
            // Arrange
            Mock<ILogger<TasksController>> mockLogger = new Mock<ILogger<TasksController>>();
            Mock<ITaskListRepository> mockTaskListRepository = new Mock<ITaskListRepository>();
            Mock<IStatusRepository> mockStatusRepository = new Mock<IStatusRepository>();
            Mock<ILocationRepository> mockLocationRepository = new Mock<ILocationRepository>();
            Mock<IIdentityRepository> mockIdentityRepository = new Mock<IIdentityRepository>();

            mockTaskListRepository.Setup(m => m.GetTasks).Returns((new Task[] {
                new Task { TaskId = 1, Description = "T1", StatusId = 1, LocationId = 1, UserId = "1", Location = new Location { Description = "L1" }, Status = new Status { Name = "Open" }, User = new AspNetUser { FullName = "Joe" } },
                new Task { TaskId = 1, Description = "T2", StatusId = 1, LocationId = 1, UserId = "1", Location = new Location { Description = "L2" }, Status = new Status { Name = "Open" }, User = new AspNetUser { FullName = "Joe" } },
                new Task { TaskId = 1, Description = "T3", StatusId = 1, LocationId = 1, UserId = "1", Location = new Location { Description = "L3" }, Status = new Status { Name = "Open" }, User = new AspNetUser { FullName = "Joe" } },
                new Task { TaskId = 1, Description = "T4", StatusId = 1, LocationId = 1, UserId = "1", Location = new Location { Description = "L4" }, Status = new Status { Name = "Open" }, User = new AspNetUser { FullName = "Joe" } },
                new Task { TaskId = 1, Description = "T5", StatusId = 1, LocationId = 1, UserId = "1", Location = new Location { Description = "L5" }, Status = new Status { Name = "Open" }, User = new AspNetUser { FullName = "Joe" } }
            }).AsQueryable<Task>());

            TasksController controller = new TasksController(mockLogger.Object, mockTaskListRepository.Object, mockStatusRepository.Object, mockLocationRepository.Object, mockIdentityRepository.Object);
            controller.PageSize = 3;

            // Act
            TasksListViewModel result = controller.List(string.Empty, "description", 1).ViewData.Model as TasksListViewModel;

            // Assert
            Task[] TaskArray = result.Tasks.ToArray();
            Assert.IsTrue(TaskArray.Length == 3);
            Assert.AreEqual("T1", TaskArray[0].Description);
            Assert.AreEqual("T2", TaskArray[1].Description);
            Assert.AreEqual("T3", TaskArray[2].Description);
        }

        [TestMethod]
        public void Task_Can_Add_Valid_Changes()
        {
            // Arrange
            TaskViewModel taskViewModel = new TaskViewModel { Task = new Task { TaskId = 1, Description = "T1", StatusId = 1, LocationId = 1, UserId = "1" }, Statuses = null, Locations = null, Users = null };

            Mock<ILogger<TasksController>> mockLogger = new Mock<ILogger<TasksController>>();
            Mock<ITaskListRepository> mockTaskListRepository = new Mock<ITaskListRepository>();
            Mock<IStatusRepository> mockStatusRepository = new Mock<IStatusRepository>();
            Mock<ILocationRepository> mockLocationRepository = new Mock<ILocationRepository>();
            Mock<IIdentityRepository> mockIdentityRepository = new Mock<IIdentityRepository>();
            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();

            TasksController controller = new TasksController(mockLogger.Object, mockTaskListRepository.Object, mockStatusRepository.Object, mockLocationRepository.Object, mockIdentityRepository.Object)
            {
                TempData = tempData.Object
            };
            
            // Act
            IActionResult result = controller.Create(taskViewModel);

            // Assert
            mockTaskListRepository.Verify(m => m.AddTask(taskViewModel.Task));

            Assert.AreEqual("List", (result as RedirectToActionResult).ActionName);
        }

        [TestMethod]
        public void Task_Cannot_Add_Invalid_Changes()
        {
            // Arrange
            TaskViewModel taskViewModel = new TaskViewModel { Task = new Task { TaskId = 1, Description = "T1", StatusId = 1, LocationId = 1, UserId = "1" }, Statuses = null, Locations = null, Users = null };

            Mock<ILogger<TasksController>> mockLogger = new Mock<ILogger<TasksController>>();
            Mock<ITaskListRepository> mockTaskListRepository = new Mock<ITaskListRepository>();
            Mock<IStatusRepository> mockStatusRepository = new Mock<IStatusRepository>();
            Mock<ILocationRepository> mockLocationRepository = new Mock<ILocationRepository>();
            Mock<IIdentityRepository> mockIdentityRepository = new Mock<IIdentityRepository>();

            TasksController controller = new TasksController(mockLogger.Object, mockTaskListRepository.Object, mockStatusRepository.Object, mockLocationRepository.Object, mockIdentityRepository.Object);
            controller.ModelState.AddModelError("error", "error");

            // Act
            IActionResult result = controller.Create(taskViewModel);

            // Assert
            mockTaskListRepository.Verify(m => m.AddTask(It.IsAny<Task>()), Times.Never());
        }

        [TestMethod]
        public void Task_Can_Edit_Valid_Changes()
        {
            // Arrange
            Mock<ILogger<TasksController>> mockLogger = new Mock<ILogger<TasksController>>();
            Mock<ITaskListRepository> mockTaskListRepository = new Mock<ITaskListRepository>();
            Mock<IStatusRepository> mockStatusRepository = new Mock<IStatusRepository>();
            Mock<ILocationRepository> mockLocationRepository = new Mock<ILocationRepository>();
            Mock<IIdentityRepository> mockIdentityRepository = new Mock<IIdentityRepository>();
            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();

            mockTaskListRepository.Setup(m => m.GetTasks).Returns(new Task[] {
                new Task { TaskId = 1, Description = "T1", StatusId = 1, LocationId = 1, UserId = "1" },
                new Task { TaskId = 1, Description = "T2", StatusId = 1, LocationId = 1, UserId = "1"  },
                new Task { TaskId = 1, Description = "T3", StatusId = 1, LocationId = 1, UserId = "1"  }
            }.AsQueryable<Task>());

            TasksController controller = new TasksController(mockLogger.Object, mockTaskListRepository.Object, mockStatusRepository.Object, mockLocationRepository.Object, mockIdentityRepository.Object)
            {
                TempData = tempData.Object
            };

            // Act
            IActionResult result1 = controller.Edit(new TaskViewModel { Task = new Task { TaskId = 1, Description = "T1", StatusId = 1, LocationId = 1, UserId = "1" }, Statuses = null, Locations = null, Users = null });
            IActionResult result2 = controller.Edit(new TaskViewModel { Task = new Task { TaskId = 1, Description = "T2", StatusId = 1, LocationId = 1, UserId = "1" }, Statuses = null, Locations = null, Users = null });
            IActionResult result3 = controller.Edit(new TaskViewModel { Task = new Task { TaskId = 1, Description = "T3", StatusId = 1, LocationId = 1, UserId = "1" }, Statuses = null, Locations = null, Users = null });

            // Assert
            Assert.AreEqual("List", (result1 as RedirectToActionResult).ActionName);
            Assert.AreEqual("List", (result2 as RedirectToActionResult).ActionName);
            Assert.AreEqual("List", (result3 as RedirectToActionResult).ActionName);
        }

        [TestMethod]
        public void Task_Cannot_Edit_Invalid_Changes()
        {
            // Arrange
            TaskViewModel taskViewModel = new TaskViewModel { Task = new Task { TaskId = 1, Description = "T1", StatusId = 1, LocationId = 1, UserId = "1" }, Statuses = null, Locations = null, Users = null };

            Mock<ILogger<TasksController>> mockLogger = new Mock<ILogger<TasksController>>();
            Mock<ITaskListRepository> mockTaskListRepository = new Mock<ITaskListRepository>();
            Mock<IStatusRepository> mockStatusRepository = new Mock<IStatusRepository>();
            Mock<ILocationRepository> mockLocationRepository = new Mock<ILocationRepository>();
            Mock<IIdentityRepository> mockIdentityRepository = new Mock<IIdentityRepository>();

            TasksController controller = new TasksController(mockLogger.Object, mockTaskListRepository.Object, mockStatusRepository.Object, mockLocationRepository.Object, mockIdentityRepository.Object);
            controller.ModelState.AddModelError("error", "error");

            // Act
            IActionResult result = controller.Edit(taskViewModel);

            // Assert
            mockTaskListRepository.Verify(m => m.EditTask(It.IsAny<Task>()), Times.Never());
        }

        [TestMethod]
        public void Task_Cannot_Edit_Nonexistent()
        {
            // Arrange
            Mock<ILogger<TasksController>> mockLogger = new Mock<ILogger<TasksController>>();
            Mock<ITaskListRepository> mockTaskListRepository = new Mock<ITaskListRepository>();
            Mock<IStatusRepository> mockStatusRepository = new Mock<IStatusRepository>();
            Mock<ILocationRepository> mockLocationRepository = new Mock<ILocationRepository>();
            Mock<IIdentityRepository> mockIdentityRepository = new Mock<IIdentityRepository>();

            TasksController controller = new TasksController(mockLogger.Object, mockTaskListRepository.Object, mockStatusRepository.Object, mockLocationRepository.Object, mockIdentityRepository.Object);

            // Act
            Task result = GetViewModel<Task>(controller.Edit(4));

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Task_Can_Delete()
        {
            // Arrange
            Task Task = new Task { TaskId = 1, Description = "T2", StatusId = 1, LocationId = 1, UserId = "1" };

            Mock<ILogger<TasksController>> mockLogger = new Mock<ILogger<TasksController>>();
            Mock<ITaskListRepository> mockTaskListRepository = new Mock<ITaskListRepository>();
            Mock<IStatusRepository> mockStatusRepository = new Mock<IStatusRepository>();
            Mock<ILocationRepository> mockLocationRepository = new Mock<ILocationRepository>();
            Mock<IIdentityRepository> mockIdentityRepository = new Mock<IIdentityRepository>();
            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();

            mockTaskListRepository.Setup(m => m.GetTasks).Returns(new Task[] {
                new Task { TaskId = 1, Description = "T1", StatusId = 1, LocationId = 1, UserId = "1"  },
                Task,
                new Task { TaskId = 1, Description = "T3", StatusId = 1, LocationId = 1, UserId = "1"  }
            }.AsQueryable<Task>());

            TasksController controller = new TasksController(mockLogger.Object, mockTaskListRepository.Object, mockStatusRepository.Object, mockLocationRepository.Object, mockIdentityRepository.Object)
            {
                TempData = tempData.Object
            };

            // Act
            controller.Delete(Task.TaskId);

            // Assert
            mockTaskListRepository.Verify(m => m.DeleteTask(Task.TaskId));
        }

        private T GetViewModel<T>(IActionResult result) where T : class
        {
            return (result as ViewResult)?.ViewData.Model as T;
        }
    }
}
