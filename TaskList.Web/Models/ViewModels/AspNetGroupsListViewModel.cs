using System.Collections.Generic;
using TaskList.DAL.Models;

namespace TaskList.Web.Models.ViewModels
{
    public class AspNetGroupsListViewModel
    {
        public IEnumerable<AspNetGroup> AspNetGroups { get; set; }

        public PagingInfo PagingInfo { get; set; }
    }
}
