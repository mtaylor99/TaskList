using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TaskList.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public IEnumerable<AspNetUser> Get()
        {
            var currentUser = HttpContext.User;
            var resultAspNetUserList = new AspNetUser[] {
                new AspNetUser { Name = "Ray Bradbury" ,Title = "Web Developer" },
                new AspNetUser { Name = "Gabriel García Márquez", Title = "Tester" },
                new AspNetUser { Name = "George Orwell", Title = "Manager" },
                new AspNetUser { Name = "Anais Nin", Title = "Project Owner" }
            };

            return resultAspNetUserList;
        }

        public class AspNetUser
        {
            public string Name { get; set; }
            public string Title { get; set; }
        }
    }
}
