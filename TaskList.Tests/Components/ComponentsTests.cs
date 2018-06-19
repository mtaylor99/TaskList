using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TaskList.Web.Components;

namespace TaskList.Tests.BusinessSetup
{
    [TestClass]
    public class ComponentsTests
    {
        [TestMethod]
        public void Components_Logged_In_User()
        {
            // Arrange
            Mock<IHttpContextAccessor> mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            Mock<ILogger<LoggedInUserViewComponent>> mockLogger = new Mock<ILogger<LoggedInUserViewComponent>>();

            mockHttpContextAccessor.Setup(x => x.HttpContext.User.Identity.Name).Returns("Administrator");

            LoggedInUserViewComponent target = new LoggedInUserViewComponent(mockHttpContextAccessor.Object, mockLogger.Object);

            //Act
            string result = (target.Invoke() as ViewViewComponentResult).ViewData.Model.ToString();

            //Assert
            Assert.AreEqual("Administrator", result);
        }

    }
}
