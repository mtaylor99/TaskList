using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using TaskList.BL.Domain;
using TaskList.DAL.Interfaces;
using TaskList.Web.Attribute.RequiresRole;
using TaskList.Web.Models.ViewModels;

namespace TaskList.Web.Areas.Project.Controllers
{
    [Authorize]
    public class TasksController : Controller
    {
        private ILogger<TasksController> logger;
        private ITaskListRepository taskRepository;
        private IStatusRepository statusRepository;
        private ILocationRepository locationsRepository;
        private IIdentityRepository identityRepository;
        Tasks tasks;
        Statuses statuses;
        Locations locations;
        Identity identity;
        public int PageSize = 25;

        public TasksController(ILogger<TasksController> log, ITaskListRepository taskRepo, IStatusRepository statusRepo, ILocationRepository locationRepo, IIdentityRepository identityRepo)
        {
            logger = log;
            taskRepository = taskRepo;
            statusRepository = statusRepo;
            locationsRepository = locationRepo;
            identityRepository = identityRepo;
            tasks = new Tasks(logger, taskRepository);
            statuses = new Statuses(logger, statusRepository);
            locations = new Locations(logger, locationsRepository);
            identity = new Identity(logger, identityRepository);

            logger.LogInformation("Tasks Controller");
        }

        public ViewResult List(string search, string sortBy, int page = 1)
        {
            var tasksListViewModel = new TasksListViewModel();

            tasksListViewModel.Search = search;

            tasksListViewModel.DescriptionSort = string.IsNullOrEmpty(sortBy) ? "description_desc" : "";
            tasksListViewModel.DescriptionSort = sortBy == "description" ? "description_desc" : "description";
            tasksListViewModel.StatusSort = sortBy == "status" ? "status_desc" : "status";

            tasksListViewModel.Tasks = tasks.GetTasks(search, sortBy, page, PageSize);

            tasksListViewModel.PagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = tasks.GetTasksCount(search)
            };

            return View(tasksListViewModel);
        }

        public ViewResult Create()
        {
            var taskViewModel = new TaskViewModel();

            taskViewModel.Statuses = statuses.GetStatuses().Select(s => new SelectListItem() { Value = s.StatusId.ToString(), Text = s.Name });
            taskViewModel.Locations = locations.GetLocations().Select(l => new SelectListItem() { Value = l.LocationId.ToString(), Text = l.Description });
            taskViewModel.Users = identity.GetAspNetUsers().Select(u => new SelectListItem() { Value = u.Id, Text = u.UserName });

            return View(taskViewModel);
        }

        [HttpPost]
        public IActionResult Create(TaskViewModel taskViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = tasks.AddTask(taskViewModel.Task);

                if (result)
                {
                    TempData["message"] = $"Task has been created.";
                }

                return RedirectToAction("List");
            }
            else
            {
                taskViewModel.Statuses = statuses.GetStatuses().Select(s => new SelectListItem() { Value = s.StatusId.ToString(), Text = s.Name });
                taskViewModel.Locations = locations.GetLocations().Select(l => new SelectListItem() { Value = l.LocationId.ToString(), Text = l.Description });
                taskViewModel.Users = identity.GetAspNetUsers().Select(u => new SelectListItem() { Value = u.Id, Text = u.UserName });

                return View(taskViewModel);
            }
        }

        public ViewResult Edit(int id)
        {
            var taskViewModel = new TaskViewModel();

            taskViewModel.Task = tasks.GetTask(id);
            taskViewModel.Statuses = statuses.GetStatuses().Select(s => new SelectListItem() { Value = s.StatusId.ToString(), Text = s.Name });
            taskViewModel.Locations = locations.GetLocations().Select(l => new SelectListItem() { Value = l.LocationId.ToString(), Text = l.Description });
            taskViewModel.Users = identity.GetAspNetUsers().Select(u => new SelectListItem() { Value = u.Id, Text = u.UserName });

            return View(taskViewModel);
        }

        [HttpPost]
        public IActionResult Edit(TaskViewModel taskViewModel)
        {
            if (ModelState.IsValid)
            {
                tasks.EditTask(taskViewModel.Task);

                TempData["message"] = $"Task has been edited.";

                return RedirectToAction("List");
            }
            else
            {
                taskViewModel.Statuses = statuses.GetStatuses().Select(s => new SelectListItem() { Value = s.StatusId.ToString(), Text = s.Name });
                taskViewModel.Locations = locations.GetLocations().Select(l => new SelectListItem() { Value = l.LocationId.ToString(), Text = l.Description });
                taskViewModel.Users = identity.GetAspNetUsers().Select(u => new SelectListItem() { Value = u.Id, Text = u.UserName });

                return View(taskViewModel);
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (ModelState.IsValid)
            {
                var result = tasks.DeleteTask(id);

                if (result)
                {
                    TempData["message"] = $"Task has been deleted.";
                }

                return RedirectToAction("List");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public IActionResult Action(string selectedGridIds)
        {
            return RedirectToAction("List");
        }

        public IActionResult Search(string searchString)
        {
            return RedirectToAction("List");
        }
    }
}
