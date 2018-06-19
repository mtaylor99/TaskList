using System.Linq;
using TaskList.DAL.DbContexts;
using TaskList.DAL.Interfaces;
using TaskList.DAL.Models;

namespace TaskList.DAL.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private ApplicationDbContext context;

        public LocationRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<Location> GetLocations => context.Location;

        public Location GetLocation(int locationId)
        {
            return context.Location.FirstOrDefault(l => l.LocationId == locationId);
        }

        public int AddLocation(Location location)
        {
            context.Location.Add(location);
            context.SaveChanges();

            return location.LocationId;
        }

        public void EditLocation(Location location)
        {
            if (location.LocationId > 0)
            {
                Location dbEntry = context.Location.FirstOrDefault(l => l.LocationId == location.LocationId);

                if (dbEntry != null)
                {
                    dbEntry.Description = location.Description;
                    dbEntry.ParentId = location.ParentId;
                    context.SaveChanges();
                }
            }
        }

        public Location DeleteLocation(int locationId)
        {
            Location dbEntry = context.Location.FirstOrDefault(l => l.LocationId == locationId);

            if (dbEntry != null)
            {
                context.Location.Remove(dbEntry);
                context.SaveChanges();
            }

            return dbEntry;
        }
    }
}
