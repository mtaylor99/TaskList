using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaskList.BL.Domain;
using TaskList.DAL.Interfaces;
using TaskList.DAL.Models;
using TaskList.Web.Models.ViewModels;

namespace TaskList.Web.Areas.BusinessSetup.Controllers
{
    public class StatusesController : Controller
    {
        private ILogger<StatusesController> logger;
        private IStatusRepository repository;
        Statuses statuses;
        public int PageSize = 4;

        public StatusesController(ILogger<StatusesController> log, IStatusRepository repo)
        {
            logger = log;
            repository = repo;
            statuses = new Statuses(logger, repository);

            logger.LogInformation("Statuses Controller");
        }

        public ViewResult List(int page = 1)
        {
            var statusesListViewModel = new StatusesListViewModel();

            statusesListViewModel.Statuses = statuses.GetStatuses(page, PageSize);

            statusesListViewModel.PagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = statuses.GetStatusesCount()
            };

            return View(statusesListViewModel);
        }

        public ViewResult Create() => View();

        [HttpPost]
        public IActionResult Create(Status status)
        {
            if (ModelState.IsValid)
            {
                var result = statuses.AddStatus(status);

                if (result)
                {
                    TempData["message"] = $"Status has been created.";
                }

                return RedirectToAction("List");
            }
            else
            {
                return View(status);
            }
        }

        public ViewResult Edit(int id)
        {
            return View(statuses.GetStatus(id));
        }

        [HttpPost]
        public IActionResult Edit(Status status)
        {
            if (ModelState.IsValid)
            {
                var result = statuses.EditStatus(status);

                if (result)
                {
                    TempData["message"] = $"Status has been edited.";
                }

                return RedirectToAction("List");
            }
            else
            {
                return View(status);
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (ModelState.IsValid)
            {
                var result = statuses.DeleteStatus(id);

                if (result)
                {
                    TempData["message"] = $"Status has been deleted.";
                }

                return RedirectToAction("List");
            }
            else
            {
                return View();
            }
        }
    }
}