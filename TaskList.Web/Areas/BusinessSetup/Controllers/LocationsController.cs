using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaskList.BL.Domain;
using TaskList.DAL.Interfaces;
using TaskList.DAL.Models;
using TaskList.Web.Models.ViewModels;

namespace TaskList.Web.Areas.BusinessSetup.Controllers
{
    public class LocationsController : Controller
    {
        private ILogger<LocationsController> logger;
        private ILocationRepository repository;
        Locations locations;
        public int PageSize = 4;

        public LocationsController(ILogger<LocationsController> log, ILocationRepository repo)
        {
            logger = log;
            repository = repo;
            locations = new Locations(logger, repository);

            logger.LogInformation("Locations Controller");
        }

        public ViewResult List(int page = 1)
        {
            var locationsListViewModel = new LocationsListViewModel();

            locationsListViewModel.Locations = locations.GetLocations(page, PageSize);

            locationsListViewModel.PagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = locations.GetLocationsCount()
            };

            return View(locationsListViewModel);
        }

        public ViewResult Create() => View();

        [HttpPost]
        public IActionResult Create(Location location)
        {
            if (ModelState.IsValid)
            {
                var result = locations.AddLocation(location);

                if (result)
                {
                    TempData["message"] = $"Location has been created.";
                }

                return RedirectToAction("List");
            }
            else
            {
                return View(location);
            }
        }

        public ViewResult Edit(int id)
        {
            return View(locations.GetLocation(id));
        }

        [HttpPost]
        public IActionResult Edit(Location location)
        {
            if (ModelState.IsValid)
            {
                var result = locations.EditLocation(location);

                if (result)
                {
                    TempData["message"] = $"Location has been edited.";
                }

                return RedirectToAction("List");
            }
            else
            {
                return View(location);
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (ModelState.IsValid)
            {
                var result = locations.DeleteLocation(id);

                if (result)
                {
                    TempData["message"] = $"Location has been deleted.";
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