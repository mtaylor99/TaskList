using System.Linq;
using TaskList.DAL.Models;

namespace TaskList.BL.Interaces
{
    public interface ILocations
    {
        IQueryable<Location> GetLocations();

        IQueryable<Location> GetLocations(int page, int pageSize);

        int GetLocationsCount();

        Location GetLocation(int locationId);

        bool AddLocation(Location location);

        bool EditLocation(Location location);

        bool DeleteLocation(int locationId);
    }
}
