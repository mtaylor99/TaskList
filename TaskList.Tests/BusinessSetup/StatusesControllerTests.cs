using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TaskList.DAL.Interfaces;
using TaskList.DAL.Models;
using TaskList.Web.Areas.BusinessSetup.Controllers;
using TaskList.Web.Models.ViewModels;

namespace TaskList.Tests.BusinessSetup
{
    [TestClass]
    public class StatusesControllerTests
    {
        [TestMethod]
        public void Status_Can_Paginate()
        {
            // Arrange
            Mock<ILogger<StatusesController>> mockLogger = new Mock<ILogger<StatusesController>>();
            Mock<IStatusRepository> mockStatusRepository = new Mock<IStatusRepository>();

            mockStatusRepository.Setup(m => m.GetStatuses).Returns((new Status[] {
                new Status { StatusId = 1, Name = "S1" },
                new Status { StatusId = 2, Name = "S2" },
                new Status { StatusId = 3, Name = "S3" },
                new Status { StatusId = 4, Name = "S4" },
                new Status { StatusId = 5, Name = "S5" }
            }).AsQueryable<Status>());

            StatusesController controller = new StatusesController(mockLogger.Object, mockStatusRepository.Object);
            controller.PageSize = 3;

            // Act
            StatusesListViewModel result = controller.List(2).ViewData.Model as StatusesListViewModel;

            // Assert
            Status[] statusArray = result.Statuses.ToArray();
            Assert.IsTrue(statusArray.Length == 2);
            Assert.AreEqual("S4", statusArray[0].Name);
            Assert.AreEqual("S5", statusArray[1].Name);
        }

        [TestMethod]
        public void Status_Can_Send_Pagination()
        {
            // Arrange
            Mock<ILogger<StatusesController>> mockLogger = new Mock<ILogger<StatusesController>>();
            Mock<IStatusRepository> mockStatusRepository = new Mock<IStatusRepository>();
            mockStatusRepository.Setup(m => m.GetStatuses).Returns((new Status[] {
                new Status { StatusId = 1, Name = "S1" },
                new Status { StatusId = 2, Name = "S2" },
                new Status { StatusId = 3, Name = "S3" },
                new Status { StatusId = 4, Name = "S4" },
                new Status { StatusId = 5, Name = "S5" }
            }).AsQueryable<Status>());

            StatusesController controller = new StatusesController(mockLogger.Object, mockStatusRepository.Object);
            controller.PageSize = 3;

            // Act
            StatusesListViewModel result = controller.List(2).ViewData.Model as StatusesListViewModel;

            // Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(2, pageInfo.CurrentPage);
            Assert.AreEqual(3, pageInfo.ItemsPerPage);
            Assert.AreEqual(5, pageInfo.TotalItems);
            Assert.AreEqual(2, pageInfo.TotalPages);
        }

        [TestMethod]
        public void Status_Can_Add_Valid_Changes()
        {
            // Arrange
            Status status = new Status { StatusId = 2, Name = "Test" };

            Mock<ILogger<StatusesController>> mockLogger = new Mock<ILogger<StatusesController>>();
            Mock<IStatusRepository> mockStatusRepository = new Mock<IStatusRepository>();
            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();

            StatusesController controller = new StatusesController(mockLogger.Object, mockStatusRepository.Object)
            {
                TempData = tempData.Object
            };
            
            // Act
            IActionResult result = controller.Create(status);

            // Assert
            mockStatusRepository.Verify(m => m.AddStatus(status));

            Assert.AreEqual("List", (result as RedirectToActionResult).ActionName);
        }

        [TestMethod]
        public void Status_Cannot_Add_Invalid_Changes()
        {
            // Arrange
            Status status = new Status { StatusId = 2, Name = "" };

            Mock<ILogger<StatusesController>> mockLogger = new Mock<ILogger<StatusesController>>();
            Mock<IStatusRepository> mockStatusRepository = new Mock<IStatusRepository>();

            StatusesController controller = new StatusesController(mockLogger.Object, mockStatusRepository.Object);
            controller.ModelState.AddModelError("error", "error");

            // Act
            IActionResult result = controller.Create(status);

            // Assert
            mockStatusRepository.Verify(m => m.AddStatus(It.IsAny<Status>()), Times.Never());
        }

        [TestMethod]
        public void Status_Can_Edit_Valid_Changes()
        {
            // Arrange
            Mock<ILogger<StatusesController>> mockLogger = new Mock<ILogger<StatusesController>>();
            Mock<IStatusRepository> mockStatusRepository = new Mock<IStatusRepository>();
            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();

            mockStatusRepository.Setup(m => m.GetStatuses).Returns(new Status[] {
                new Status {StatusId = 1, Name = "S1"},
                new Status {StatusId = 2, Name = "S2"},
                new Status {StatusId = 3, Name = "S3"},
            }.AsQueryable<Status>());

            StatusesController controller = new StatusesController(mockLogger.Object, mockStatusRepository.Object)
            {
                TempData = tempData.Object
            };

            // Act
            IActionResult result1 = controller.Edit(new Status { StatusId = 1, Name = "S1" });
            IActionResult result2 = controller.Edit(new Status { StatusId = 2, Name = "S2" });
            IActionResult result3 = controller.Edit(new Status { StatusId = 3, Name = "S3" });

            // Assert
            Assert.AreEqual("List", (result1 as RedirectToActionResult).ActionName);
            Assert.AreEqual("List", (result2 as RedirectToActionResult).ActionName);
            Assert.AreEqual("List", (result3 as RedirectToActionResult).ActionName);
        }

        [TestMethod]
        public void Status_Cannot_Edit_Invalid_Changes()
        {
            // Arrange
            Status status = new Status { StatusId = 2, Name = "" };

            Mock<ILogger<StatusesController>> mockLogger = new Mock<ILogger<StatusesController>>();
            Mock<IStatusRepository> mockStatusRepository = new Mock<IStatusRepository>();

            StatusesController controller = new StatusesController(mockLogger.Object, mockStatusRepository.Object);
            controller.ModelState.AddModelError("error", "error");

            // Act
            IActionResult result = controller.Edit(status);

            // Assert
            mockStatusRepository.Verify(m => m.EditStatus(It.IsAny<Status>()), Times.Never());
        }

        [TestMethod]
        public void Status_Cannot_Edit_Nonexistent()
        {
            // Arrange
            Mock<ILogger<StatusesController>> mockLogger = new Mock<ILogger<StatusesController>>();
            Mock<IStatusRepository> mockStatusRepository = new Mock<IStatusRepository>();

            StatusesController controller = new StatusesController(mockLogger.Object, mockStatusRepository.Object);

            // Act
            Status result = GetViewModel<Status>(controller.Edit(4));

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Status_Can_Delete()
        {
            // Arrange
            Status status = new Status { StatusId = 2, Name = "S2" };

            Mock<ILogger<StatusesController>> mockLogger = new Mock<ILogger<StatusesController>>();
            Mock<IStatusRepository> mockStatusRepository = new Mock<IStatusRepository>();
            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();

            mockStatusRepository.Setup(m => m.GetStatuses).Returns(new Status[] {
                new Status {StatusId = 1, Name = "S1"},
                status,
                new Status {StatusId = 3, Name = "S3"},
            }.AsQueryable<Status>());

            StatusesController controller = new StatusesController(mockLogger.Object, mockStatusRepository.Object)
            {
                TempData = tempData.Object
            };

            // Act
            controller.Delete(status.StatusId);

            // Assert
            mockStatusRepository.Verify(m => m.DeleteStatus(status.StatusId));
        }

        private T GetViewModel<T>(IActionResult result) where T : class
        {
            return (result as ViewResult)?.ViewData.Model as T;
        }
    }
}
