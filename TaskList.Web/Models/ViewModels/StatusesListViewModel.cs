using System.Collections.Generic;
using TaskList.DAL.Models;

namespace TaskList.Web.Models.ViewModels
{
    public class StatusesListViewModel
    {
        public IEnumerable<Status> Statuses { get; set; }

        public PagingInfo PagingInfo { get; set; }
    }
}
