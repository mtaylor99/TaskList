using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaskList.DAL.Interfaces;
using TaskList.BL.Domain;
using Newtonsoft.Json;

namespace TaskList.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class LocationsController : Controller
    {
        private ILogger<LocationsController> logger;
        private ILocationRepository locationRepository;
        Locations locations;

        public LocationsController(ILogger<LocationsController> log, ILocationRepository locationRepo)
        {
            logger = log;
            locationRepository = locationRepo;
            locations = new Locations(logger, locationRepository);

            logger.LogInformation("Api - Locations Controller");
        }

        [HttpGet]
        public IActionResult Get([FromHeader] string page, [FromHeader] string pageSize)
        {
            if (page == null) page = "1";
            if (pageSize == null) pageSize = "9999";

            var data = locations.GetLocations(Convert.ToInt32(page), Convert.ToInt32(pageSize));

            var json = JsonConvert.SerializeObject(data);

            return Ok(json);
        }
    }
}
