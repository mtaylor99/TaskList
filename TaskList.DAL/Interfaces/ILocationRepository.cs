using System.Linq;
using TaskList.DAL.Models;

namespace TaskList.DAL.Interfaces
{
    public interface ILocationRepository
    {
        IQueryable<Location> GetLocations { get; }

        Location GetLocation(int locationId);

        int AddLocation(Location location);

        void EditLocation(Location location);

        Location DeleteLocation(int locationId);
    }
}
