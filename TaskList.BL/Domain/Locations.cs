using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using TaskList.BL.Helpers;
using TaskList.BL.Interaces;
using TaskList.DAL.Interfaces;
using TaskList.DAL.Models;

namespace TaskList.BL.Domain
{
    public class Locations : ILocations
    {
        private ILogger logger;
        private ILocationRepository locationRepository;

        public Locations(ILogger log, ILocationRepository locationRepo)
        {
            logger = log;
            locationRepository = locationRepo;

            logger.LogInformation("Locations Business Logic - Constructor");
        }

        public IQueryable<Location> GetLocations()
        {
            logger.LogInformation("Locations Business Logic - GetLocations");

            return locationRepository.GetLocations;
        }

        public IQueryable<Location> GetLocations(int page, int pageSize)
        {
            logger.LogInformation("Locations Business Logic - GetLocations");

            return locationRepository.GetLocations
                .OrderBy(l => l.LocationId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
        }

        public IEnumerable<LocationTreeNode> GetLocationTree()
        {
            logger.LogInformation("Locations Business Logic - GetLocationTree");

            var locations = locationRepository.GetLocations
                .OrderBy(l => l.LocationId)
                .ToList();

            return locations.GenerateTree(c => c.LocationId, c => c.ParentId);
        }

        public int GetLocationsCount()
        {
            logger.LogInformation("Locations Business Logic - GetLocationsCount");

            return locationRepository.GetLocations
                .Count();
        }

        public Location GetLocation(int locationId)
        {
            logger.LogInformation("Locations Business Logic - GetStatus");

            return locationRepository.GetLocation(locationId);
        }

        public bool AddLocation(Location location)
        {
            logger.LogInformation("Locations Business Logic - AddLocation");

            locationRepository.AddLocation(location);

            return true;
        }

        public bool EditLocation(Location location)
        {
            logger.LogInformation("Locations Business Logic - EditLocation");

            locationRepository.EditLocation(location);

            return true;
        }

        public bool DeleteLocation(int locationId)
        {
            logger.LogInformation("Locations Business Logic - DeleteLocation");

            locationRepository.DeleteLocation(locationId);

            return true;
        }
    }
}
