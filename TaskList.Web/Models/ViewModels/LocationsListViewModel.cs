using System.Collections.Generic;
using TaskList.DAL.Models;

namespace TaskList.Web.Models.ViewModels
{
    public class LocationsListViewModel
    {
        public IEnumerable<Location> Locations { get; set; }

        public PagingInfo PagingInfo { get; set; }
    }
}
