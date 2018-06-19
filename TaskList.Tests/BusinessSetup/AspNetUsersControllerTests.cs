using System;
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
    public class AspNetUsersControllerTests
    {
        List<SelectListItem> aspNetGroupsList;
        string[] selectedGroups;

        [TestInitialize]
        public void Initialize()
        {
            aspNetGroupsList = new List<SelectListItem>();
            aspNetGroupsList.Add(new SelectListItem { Text = "G1", Value = "1" });
            aspNetGroupsList.Add(new SelectListItem { Text = "G2", Value = "2" });

            selectedGroups = new string[] { "1", "2" };
        }

        [TestMethod]
        public void AspNetUsers_Can_Paginate()
        {
            // Arrange
            Mock<ILogger<AspNetUsersController>> mockLogger = new Mock<ILogger<AspNetUsersController>>();
            Mock<IIdentityRepository> mockIdentityRepository = new Mock<IIdentityRepository>();

            mockIdentityRepository.Setup(m => m.GetAspNetUsers).Returns((new AspNetUser[] {
                new AspNetUser { UserName = "U1", Email = "U1@test.com", FullName = "U1", BirthDate = DateTime.Now },
                new AspNetUser { UserName = "U2", Email = "U2@test.com", FullName = "U2", BirthDate = DateTime.Now },
                new AspNetUser { UserName = "U3", Email = "U3@test.com", FullName = "U3", BirthDate = DateTime.Now },
                new AspNetUser { UserName = "U4", Email = "U4@test.com", FullName = "U4", BirthDate = DateTime.Now },
                new AspNetUser { UserName = "U5", Email = "U5@test.com", FullName = "U5", BirthDate = DateTime.Now }
            }).AsQueryable<AspNetUser>());

            AspNetUsersController controller = new AspNetUsersController(mockLogger.Object, mockIdentityRepository.Object);
            controller.PageSize = 3;

            // Act
            AspNetUsersListViewModel result = controller.List(2).ViewData.Model as AspNetUsersListViewModel;

            // Assert
            AspNetUser[] AspNetUserArray = result.AspNetUsers.ToArray();
            Assert.IsTrue(AspNetUserArray.Length == 2);
            Assert.AreEqual("U4", AspNetUserArray[0].UserName);
            Assert.AreEqual("U5", AspNetUserArray[1].UserName);
        }

        [TestMethod]
        public void AspNetUsers_Can_Send_Pagination()
        {
            // Arrange
            Mock<ILogger<AspNetUsersController>> mockLogger = new Mock<ILogger<AspNetUsersController>>();
            Mock<IIdentityRepository> mockIdentityRepository = new Mock<IIdentityRepository>();

            mockIdentityRepository.Setup(m => m.GetAspNetUsers).Returns((new AspNetUser[] {
                new AspNetUser { UserName = "U1", Email = "U1@test.com", FullName = "U1", BirthDate = DateTime.Now },
                new AspNetUser { UserName = "U2", Email = "U2@test.com", FullName = "U2", BirthDate = DateTime.Now },
                new AspNetUser { UserName = "U3", Email = "U3@test.com", FullName = "U3", BirthDate = DateTime.Now },
                new AspNetUser { UserName = "U4", Email = "U4@test.com", FullName = "U4", BirthDate = DateTime.Now },
                new AspNetUser { UserName = "U5", Email = "U5@test.com", FullName = "U5", BirthDate = DateTime.Now }
            }).AsQueryable<AspNetUser>());

            AspNetUsersController controller = new AspNetUsersController(mockLogger.Object, mockIdentityRepository.Object);
            controller.PageSize = 3;

            // Act
            AspNetUsersListViewModel result = controller.List(2).ViewData.Model as AspNetUsersListViewModel;

            // Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(2, pageInfo.CurrentPage);
            Assert.AreEqual(3, pageInfo.ItemsPerPage);
            Assert.AreEqual(5, pageInfo.TotalItems);
            Assert.AreEqual(2, pageInfo.TotalPages);
        }

        [TestMethod]
        public void AspNetUser_Can_Add_Valid_Changes()
        {
            // Arrange
            AspNetUsersViewModel AspNetUsersViewModel = new AspNetUsersViewModel { UserName = "Test", Email = "Test@test.com", FullName = "Test", Password = "password", AspNetGroupsList = aspNetGroupsList };

            AspNetUser aspNetUser = new AspNetUser();
            aspNetUser.UserName = "Test";
            aspNetUser.FullName = "Test";
            aspNetUser.Email = "Test@test.com";

            Mock<ILogger<AspNetUsersController>> mockLogger = new Mock<ILogger<AspNetUsersController>>();
            Mock<IIdentityRepository> mockIdentityRepository = new Mock<IIdentityRepository>();
            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();

            //mockIdentityRepository.Setup(u => u.AddAspNetUser(It.IsAny<AspNetUser>(), "password"))
            //    .Returns("0000-0000-0000-0000");

            AspNetUsersController controller = new AspNetUsersController(mockLogger.Object, mockIdentityRepository.Object)
            {
                TempData = tempData.Object
            };

            // Act
            IActionResult result = controller.Create(AspNetUsersViewModel, selectedGroups);

            // Assert
            Assert.AreEqual("List", (result as RedirectToActionResult).ActionName);
        }

        [TestMethod]
        public void AspNetUser_Cannot_Add_Invalid_Changes()
        {
            // Arrange
            AspNetUsersViewModel AspNetUsersViewModel = new AspNetUsersViewModel { UserName = "Test", Email = "Test@test.com", FullName = "Test", Password = "password", AspNetGroupsList = aspNetGroupsList };

            Mock<ILogger<AspNetUsersController>> mockLogger = new Mock<ILogger<AspNetUsersController>>();
            Mock<IIdentityRepository> mockIdentityRepository = new Mock<IIdentityRepository>();

            AspNetUsersController controller = new AspNetUsersController(mockLogger.Object, mockIdentityRepository.Object);
            controller.ModelState.AddModelError("error", "error");

            // Act
            IActionResult result = controller.Create(AspNetUsersViewModel, selectedGroups);

            // Assert
            mockIdentityRepository.Verify(m => m.AddAspNetUser(It.IsAny<AspNetUser>(), "password"), Times.Never());
        }

        [TestMethod]
        public void AspNetUser_Can_Edit_Valid_Changes()
        {
            // Arrange
            Mock<ILogger<AspNetUsersController>> mockLogger = new Mock<ILogger<AspNetUsersController>>();
            Mock<IIdentityRepository> mockIdentityRepository = new Mock<IIdentityRepository>();
            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();

            mockIdentityRepository.Setup(m => m.GetAspNetUsers).Returns(new AspNetUser[] {
                new AspNetUser { UserName = "U1", Email = "U1@test.com", FullName = "U1", BirthDate = DateTime.Now }
            }.AsQueryable<AspNetUser>());

            AspNetUsersController controller = new AspNetUsersController(mockLogger.Object, mockIdentityRepository.Object)
            {
                TempData = tempData.Object
            };

            // Act
            IActionResult result1 = controller.Edit(new AspNetUsersViewModel { UserName = "U1", Email = "U1@test.com", FullName = "U1", Password = "password", AspNetGroupsList = aspNetGroupsList }, selectedGroups);

            // Assert
            Assert.AreEqual("List", (result1 as RedirectToActionResult).ActionName);
        }

        [TestMethod]
        public void AspNetUser_Cannot_Edit_Invalid_Changes()
        {
            // Arrange
            AspNetUsersViewModel AspNetUsersViewModel = new AspNetUsersViewModel { UserName = "U1", Email = "U1@test.com", FullName = "U1", Password = "password", AspNetGroupsList = aspNetGroupsList };

            Mock<ILogger<AspNetUsersController>> mockLogger = new Mock<ILogger<AspNetUsersController>>();
            Mock<IIdentityRepository> mockIdentityRepository = new Mock<IIdentityRepository>();

            AspNetUsersController controller = new AspNetUsersController(mockLogger.Object, mockIdentityRepository.Object);
            controller.ModelState.AddModelError("error", "error");

            // Act
            IActionResult result = controller.Edit(AspNetUsersViewModel, selectedGroups);

            // Assert
            mockIdentityRepository.Verify(m => m.EditAspNetUser(It.IsAny<AspNetUser>(), "password"), Times.Never());
        }

        [TestMethod]
        public void AspNetUser_Cannot_Edit_Nonexistent()
        {
            // Arrange
            Mock<ILogger<AspNetUsersController>> mockLogger = new Mock<ILogger<AspNetUsersController>>();
            Mock<IIdentityRepository> mockIdentityRepository = new Mock<IIdentityRepository>();

            AspNetUsersController controller = new AspNetUsersController(mockLogger.Object, mockIdentityRepository.Object);

            // Act
            AspNetUser result = GetViewModel<AspNetUser>(controller.Edit("4"));

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void AspNetUser_Can_Delete()
        {
            // Arrange
            AspNetUser AspNetUser = new AspNetUser { UserName = "U1", Email = "U1@test.com", FullName = "U1", BirthDate = DateTime.Now };

            Mock<ILogger<AspNetUsersController>> mockLogger = new Mock<ILogger<AspNetUsersController>>();
            Mock<IIdentityRepository> mockIdentityRepository = new Mock<IIdentityRepository>();
            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();

            mockIdentityRepository.Setup(m => m.GetAspNetUsers).Returns(new AspNetUser[] {
                new AspNetUser { UserName = "U1", Email = "U1@test.com", FullName = "U1", BirthDate = DateTime.Now },
                AspNetUser,
                new AspNetUser { UserName = "U3", Email = "U3@test.com", FullName = "U3", BirthDate = DateTime.Now }
            }.AsQueryable<AspNetUser>());

            AspNetUsersController controller = new AspNetUsersController(mockLogger.Object, mockIdentityRepository.Object)
            {
                TempData = tempData.Object
            };

            // Act
            controller.Delete("4"); //AspNetUser.UserId

            // Assert
            mockIdentityRepository.Verify(m => m.DeleteAspNetUser("4")); //AspNetUser.UserId
        }

        private T GetViewModel<T>(IActionResult result) where T : class
        {
            return (result as ViewResult)?.ViewData.Model as T;
        }
    }
}
