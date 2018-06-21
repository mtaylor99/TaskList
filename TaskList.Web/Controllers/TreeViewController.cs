using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TaskList.BL.Domain;
using TaskList.DAL.Interfaces;
using TaskList.DAL.Models;

namespace TaskList.Web.Controllers
{
    public class TreeViewController : Controller
    {
        private ILogger<TreeViewController> logger;
        private ILocationRepository repository;
        Locations locations;

        public TreeViewController(ILogger<TreeViewController> log, ILocationRepository repo)
        {
            logger = log;
            repository = repo;
            locations = new Locations(logger, repository);

            logger.LogInformation("Locations Controller");
        }

        public IActionResult Index()
        {
            return View();
        }

        public JsonResult GetTree()
        {
            var locationTree = locations.GetLocationTree();

            var jsonLocationTree = JsonConvert.SerializeObject(locationTree);

            return Json(jsonLocationTree);
        }
    }
}