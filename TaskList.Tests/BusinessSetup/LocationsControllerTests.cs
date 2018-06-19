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
    public class LocationsControllerTests
    {
        [TestMethod]
        public void Location_Can_Paginate()
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
            controller.PageSize = 3;

            // Act
            LocationsListViewModel result = controller.List(2).ViewData.Model as LocationsListViewModel;

            // Assert
            Location[] LocationArray = result.Locations.ToArray();
            Assert.IsTrue(LocationArray.Length == 2);
            Assert.AreEqual("L4", LocationArray[0].Description);
            Assert.AreEqual("L5", LocationArray[1].Description);
        }

        [TestMethod]
        public void Location_Can_Send_Pagination()
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
            controller.PageSize = 3;

            // Act
            LocationsListViewModel result = controller.List(2).ViewData.Model as LocationsListViewModel;

            // Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(2, pageInfo.CurrentPage);
            Assert.AreEqual(3, pageInfo.ItemsPerPage);
            Assert.AreEqual(5, pageInfo.TotalItems);
            Assert.AreEqual(2, pageInfo.TotalPages);
        }

        [TestMethod]
        public void Location_Can_Add_Valid_Changes()
        {
            // Arrange
            Location Location = new Location { LocationId = 2, ParentId = 1, Description = "Test" };

            Mock<ILogger<LocationsController>> mockLogger = new Mock<ILogger<LocationsController>>();
            Mock<ILocationRepository> mockLocationRepository = new Mock<ILocationRepository>();
            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();

            LocationsController controller = new LocationsController(mockLogger.Object, mockLocationRepository.Object)
            {
                TempData = tempData.Object
            };
            
            // Act
            IActionResult result = controller.Create(Location);

            // Assert
            mockLocationRepository.Verify(m => m.AddLocation(Location));

            Assert.AreEqual("List", (result as RedirectToActionResult).ActionName);
        }

        [TestMethod]
        public void Location_Cannot_Add_Invalid_Changes()
        {
            // Arrange
            Location Location = new Location { LocationId = 2, ParentId = 1, Description = "" };

            Mock<ILogger<LocationsController>> mockLogger = new Mock<ILogger<LocationsController>>();
            Mock<ILocationRepository> mockLocationRepository = new Mock<ILocationRepository>();

            LocationsController controller = new LocationsController(mockLogger.Object, mockLocationRepository.Object);
            controller.ModelState.AddModelError("error", "error");

            // Act
            IActionResult result = controller.Create(Location);

            // Assert
            mockLocationRepository.Verify(m => m.AddLocation(It.IsAny<Location>()), Times.Never());
        }

        [TestMethod]
        public void Location_Can_Edit_Valid_Changes()
        {
            // Arrange
            Mock<ILogger<LocationsController>> mockLogger = new Mock<ILogger<LocationsController>>();
            Mock<ILocationRepository> mockLocationRepository = new Mock<ILocationRepository>();
            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();

            mockLocationRepository.Setup(m => m.GetLocations).Returns(new Location[] {
                new Location { LocationId = 1, ParentId = 0, Description = "L1" },
                new Location { LocationId = 2, ParentId = 1, Description = "L2" },
                new Location { LocationId = 3, ParentId = 1, Description = "L3" }
            }.AsQueryable<Location>());

            LocationsController controller = new LocationsController(mockLogger.Object, mockLocationRepository.Object)
            {
                TempData = tempData.Object
            };

            // Act
            IActionResult result1 = controller.Edit(new Location { LocationId = 1, ParentId = 0, Description = "L1" });
            IActionResult result2 = controller.Edit(new Location { LocationId = 2, ParentId = 1, Description = "L2" });
            IActionResult result3 = controller.Edit(new Location { LocationId = 3, ParentId = 1, Description = "L3" });

            // Assert
            Assert.AreEqual("List", (result1 as RedirectToActionResult).ActionName);
            Assert.AreEqual("List", (result2 as RedirectToActionResult).ActionName);
            Assert.AreEqual("List", (result3 as RedirectToActionResult).ActionName);
        }

        [TestMethod]
        public void Location_Cannot_Edit_Invalid_Changes()
        {
            // Arrange
            Location Location = new Location { LocationId = 2, ParentId = 1, Description = "" };

            Mock<ILogger<LocationsController>> mockLogger = new Mock<ILogger<LocationsController>>();
            Mock<ILocationRepository> mockLocationRepository = new Mock<ILocationRepository>();

            LocationsController controller = new LocationsController(mockLogger.Object, mockLocationRepository.Object);
            controller.ModelState.AddModelError("error", "error");

            // Act
            IActionResult result = controller.Edit(Location);

            // Assert
            mockLocationRepository.Verify(m => m.EditLocation(It.IsAny<Location>()), Times.Never());
        }

        [TestMethod]
        public void Location_Cannot_Edit_Nonexistent()
        {
            // Arrange
            Mock<ILogger<LocationsController>> mockLogger = new Mock<ILogger<LocationsController>>();
            Mock<ILocationRepository> mockLocationRepository = new Mock<ILocationRepository>();

            LocationsController controller = new LocationsController(mockLogger.Object, mockLocationRepository.Object);

            // Act
            Location result = GetViewModel<Location>(controller.Edit(4));

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Location_Can_Delete()
        {
            // Arrange
            Location Location = new Location { LocationId = 2, ParentId = 1, Description = "L2" };

            Mock<ILogger<LocationsController>> mockLogger = new Mock<ILogger<LocationsController>>();
            Mock<ILocationRepository> mockLocationRepository = new Mock<ILocationRepository>();
            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();

            mockLocationRepository.Setup(m => m.GetLocations).Returns(new Location[] {
                new Location {LocationId = 1, ParentId = 0, Description = "L1"},
                Location,
                new Location {LocationId = 3, ParentId = 1, Description = "L3"},
            }.AsQueryable<Location>());

            LocationsController controller = new LocationsController(mockLogger.Object, mockLocationRepository.Object)
            {
                TempData = tempData.Object
            };

            // Act
            controller.Delete(Location.LocationId);

            // Assert
            mockLocationRepository.Verify(m => m.DeleteLocation(Location.LocationId));
        }

        private T GetViewModel<T>(IActionResult result) where T : class
        {
            return (result as ViewResult)?.ViewData.Model as T;
        }
    }
}
