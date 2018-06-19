using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaskList.BL.Domain;
using TaskList.DAL.Interfaces;

namespace TaskList.Web.Components
{
    public class LoggedInUserViewComponent : ViewComponent
    {
        private readonly IHttpContextAccessor http;
        private ILogger<LoggedInUserViewComponent> logger;

        public LoggedInUserViewComponent(IHttpContextAccessor httpContextAccessor, ILogger<LoggedInUserViewComponent> log)
        {
            http = httpContextAccessor;
            logger = log;

            logger.LogInformation("Logged In User View Component");
        }

        public IViewComponentResult Invoke()
        {
            return View((object)http.HttpContext.User?.Identity?.Name);
        }
    }
}
