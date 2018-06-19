using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    public class AspNetGroupsControllerTests
    {
        List<SelectListItem> aspNetRolesList;
        string[] selectedRoles;

        [TestInitialize]
        public void Initialize()
        {
            aspNetRolesList = new List<SelectListItem>();
            aspNetRolesList.Add(new SelectListItem { Text = "R1", Value = "1" });
            aspNetRolesList.Add(new SelectListItem { Text = "R2", Value = "2" });

            selectedRoles = new string[] { "R1", "R2" };
        }

        [TestMethod]
        public void AspNetGroups_Can_Paginate()
        {
            // Arrange
            Mock<ILogger<AspNetGroupsController>> mockLogger = new Mock<ILogger<AspNetGroupsController>>();
            Mock<IIdentityRepository> mockIdentityRepository = new Mock<IIdentityRepository>();

            mockIdentityRepository.Setup(m => m.GetAspNetGroups).Returns((new AspNetGroup[] {
                new AspNetGroup { GroupId = 1, Name = "G1", Description = "G1", Active = true },
                new AspNetGroup { GroupId = 2, Name = "G2", Description = "G2", Active = true },
                new AspNetGroup { GroupId = 3, Name = "G3", Description = "G3", Active = true },
                new AspNetGroup { GroupId = 4, Name = "G4", Description = "G4", Active = true },
                new AspNetGroup { GroupId = 5, Name = "G5", Description = "G5", Active = true }
            }).AsQueryable<AspNetGroup>());

            AspNetGroupsController controller = new AspNetGroupsController(mockLogger.Object, mockIdentityRepository.Object);
            controller.PageSize = 3;

            // Act
            AspNetGroupsListViewModel result = controller.List(2).ViewData.Model as AspNetGroupsListViewModel;

            // Assert
            AspNetGroup[] AspNetGroupArray = result.AspNetGroups.ToArray();
            Assert.IsTrue(AspNetGroupArray.Length == 2);
            Assert.AreEqual("G4", AspNetGroupArray[0].Name);
            Assert.AreEqual("G5", AspNetGroupArray[1].Name);
        }

        [TestMethod]
        public void AspNetGroups_Can_Send_Pagination()
        {
            // Arrange
            Mock<ILogger<AspNetGroupsController>> mockLogger = new Mock<ILogger<AspNetGroupsController>>();
            Mock<IIdentityRepository> mockIdentityRepository = new Mock<IIdentityRepository>();

            mockIdentityRepository.Setup(m => m.GetAspNetGroups).Returns((new AspNetGroup[] {
                new AspNetGroup { GroupId = 1, Name = "G1", Description = "G1", Active = true },
                new AspNetGroup { GroupId = 2, Name = "G2", Description = "G2", Active = true },
                new AspNetGroup { GroupId = 3, Name = "G3", Description = "G3", Active = true },
                new AspNetGroup { GroupId = 4, Name = "G4", Description = "G4", Active = true },
                new AspNetGroup { GroupId = 5, Name = "G5", Description = "G5", Active = true }
            }).AsQueryable<AspNetGroup>());

            AspNetGroupsController controller = new AspNetGroupsController(mockLogger.Object, mockIdentityRepository.Object);
            controller.PageSize = 3;

            // Act
            AspNetGroupsListViewModel result = controller.List(2).ViewData.Model as AspNetGroupsListViewModel;

            // Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(2, pageInfo.CurrentPage);
            Assert.AreEqual(3, pageInfo.ItemsPerPage);
            Assert.AreEqual(5, pageInfo.TotalItems);
            Assert.AreEqual(2, pageInfo.TotalPages);
        }

        [TestMethod]
        public void AspNetGroup_Can_Add_Valid_Changes()
        {
            // Arrange
            AspNetGroupsViewModel AspNetGroupsViewModel = new AspNetGroupsViewModel { AspNetGroup = new AspNetGroup { GroupId = 1, Name = "Test", Description = "Test", Active = true }, AspNetRolesList = aspNetRolesList };

            Mock<ILogger<AspNetGroupsController>> mockLogger = new Mock<ILogger<AspNetGroupsController>>();
            Mock<IIdentityRepository> mockIdentityRepository = new Mock<IIdentityRepository>();
            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();

            AspNetGroupsController controller = new AspNetGroupsController(mockLogger.Object, mockIdentityRepository.Object)
            {
                TempData = tempData.Object
            };

            mockIdentityRepository.Setup(x => x.AddGroup(It.IsAny<AspNetGroup>())).Returns(1);

            // Act
            IActionResult result = controller.Create(AspNetGroupsViewModel, selectedRoles);

            // Assert
            mockIdentityRepository.Verify(m => m.AddGroup(AspNetGroupsViewModel.AspNetGroup));

            Assert.AreEqual("List", (result as RedirectToActionResult).ActionName);
        }

        [TestMethod]
        public void AspNetGroup_Cannot_Add_Invalid_Changes()
        {
            // Arrange
            AspNetGroupsViewModel AspNetGroupsViewModel = new AspNetGroupsViewModel { AspNetGroup = new AspNetGroup { GroupId = 1, Name = "Test", Description = "Test", Active = true }, AspNetRolesList = aspNetRolesList };

            Mock<ILogger<AspNetGroupsController>> mockLogger = new Mock<ILogger<AspNetGroupsController>>();
            Mock<IIdentityRepository> mockIdentityRepository = new Mock<IIdentityRepository>();

            AspNetGroupsController controller = new AspNetGroupsController(mockLogger.Object, mockIdentityRepository.Object);
            controller.ModelState.AddModelError("error", "error");

            mockIdentityRepository.Setup(x => x.AddGroup(It.IsAny<AspNetGroup>())).Returns(1);

            // Act
            IActionResult result = controller.Create(AspNetGroupsViewModel, selectedRoles);

            // Assert
            mockIdentityRepository.Verify(m => m.AddGroup(It.IsAny<AspNetGroup>()), Times.Never());
        }

        [TestMethod]
        public void AspNetGroup_Can_Edit_Valid_Changes()
        {
            // Arrange
            Mock<ILogger<AspNetGroupsController>> mockLogger = new Mock<ILogger<AspNetGroupsController>>();
            Mock<IIdentityRepository> mockIdentityRepository = new Mock<IIdentityRepository>();
            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();

            mockIdentityRepository.Setup(m => m.GetAspNetGroups).Returns(new AspNetGroup[] {
                new AspNetGroup { GroupId = 1, Name = "G1", Description = "G1", Active = true }
            }.AsQueryable<AspNetGroup>());

            AspNetGroupsController controller = new AspNetGroupsController(mockLogger.Object, mockIdentityRepository.Object)
            {
                TempData = tempData.Object
            };

            mockIdentityRepository.Setup(x => x.EditGroup(It.IsAny<AspNetGroup>())).Returns(1);

            // Act
            IActionResult result1 = controller.Edit(new AspNetGroupsViewModel { AspNetGroup = new AspNetGroup { GroupId = 1, Name = "G1", Description = "G1", Active = true }, AspNetRolesList = aspNetRolesList }, selectedRoles);
            
            // Assert
            Assert.AreEqual("List", (result1 as RedirectToActionResult).ActionName);
        }

        [TestMethod]
        public void AspNetGroup_Cannot_Edit_Invalid_Changes()
        {
            // Arrange
            AspNetGroupsViewModel AspNetGroupsViewModel = new AspNetGroupsViewModel { AspNetGroup = new AspNetGroup { GroupId = 1, Name = "G1", Description = "G1", Active = true }, AspNetRolesList = aspNetRolesList };

            Mock<ILogger<AspNetGroupsController>> mockLogger = new Mock<ILogger<AspNetGroupsController>>();
            Mock<IIdentityRepository> mockIdentityRepository = new Mock<IIdentityRepository>();

            AspNetGroupsController controller = new AspNetGroupsController(mockLogger.Object, mockIdentityRepository.Object);
            controller.ModelState.AddModelError("error", "error");
            // Act
            IActionResult result = controller.Edit(AspNetGroupsViewModel, selectedRoles);

            // Assert
            mockIdentityRepository.Verify(m => m.EditGroup(It.IsAny<AspNetGroup>()), Times.Never());
        }

        [TestMethod]
        public void AspNetGroup_Cannot_Edit_Nonexistent()
        {
            // Arrange
            Mock<ILogger<AspNetGroupsController>> mockLogger = new Mock<ILogger<AspNetGroupsController>>();
            Mock<IIdentityRepository> mockIdentityRepository = new Mock<IIdentityRepository>();

            AspNetGroupsController controller = new AspNetGroupsController(mockLogger.Object, mockIdentityRepository.Object);

            // Act
            AspNetGroup result = GetViewModel<AspNetGroup>(controller.Edit(4));

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void AspNetGroup_Can_Delete()
        {
            // Arrange
            AspNetGroup AspNetGroup = new AspNetGroup { GroupId = 1, Name = "G1", Description = "G1", Active = true };

            Mock<ILogger<AspNetGroupsController>> mockLogger = new Mock<ILogger<AspNetGroupsController>>();
            Mock<IIdentityRepository> mockIdentityRepository = new Mock<IIdentityRepository>();
            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();

            mockIdentityRepository.Setup(m => m.GetAspNetGroups).Returns(new AspNetGroup[] {
                new AspNetGroup { GroupId = 1, Name = "G1", Description = "G1", Active = true },
                AspNetGroup,
                new AspNetGroup { GroupId = 3, Name = "G3", Description = "G3", Active = true }
            }.AsQueryable<AspNetGroup>());

            AspNetGroupsController controller = new AspNetGroupsController(mockLogger.Object, mockIdentityRepository.Object)
            {
                TempData = tempData.Object
            };

            // Act
            controller.Delete(AspNetGroup.GroupId);

            // Assert
            mockIdentityRepository.Verify(m => m.DeleteGroup(AspNetGroup.GroupId));
        }

        private T GetViewModel<T>(IActionResult result) where T : class
        {
            return (result as ViewResult)?.ViewData.Model as T;
        }
    }
}
