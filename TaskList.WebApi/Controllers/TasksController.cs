using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaskList.DAL.Interfaces;
using TaskList.BL.Domain;
using Newtonsoft.Json;

namespace TaskList.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class TasksController : Controller
    {
        private ILogger<TasksController> logger;
        private ITaskListRepository taskRepository;
        Tasks tasks;

        public TasksController(ILogger<TasksController> log, ITaskListRepository taskRepo)
        {
            logger = log;
            taskRepository = taskRepo;
            tasks = new Tasks(logger, taskRepository);

            logger.LogInformation("Api - Tasks Controller");
        }

        [HttpGet]
        public IActionResult Get([FromHeader] string search, [FromHeader] string sortBy, [FromHeader] string page, [FromHeader] string pageSize)
        {
            if (page == null) page = "1";
            if (pageSize == null) pageSize = "9999";

            var data = tasks.GetTasks(search, sortBy, Convert.ToInt32(page), Convert.ToInt32(pageSize));

            var json = JsonConvert.SerializeObject(data);

            return Ok(json);
        }


        [HttpGet("{id}")]
        public IActionResult Get([FromHeader] int id)
        {
            var data = tasks.GetTask(id);

            var json = JsonConvert.SerializeObject(data);

            return Ok(json);
        }
    }
}
