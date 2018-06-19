using System.Collections.Generic;
using TaskList.DAL.Models;

namespace TaskList.Web.Models.ViewModels
{
    public class TasksListViewModel
    {
        public string Search { get; set; }

        public string DescriptionSort { get; set; }

        public string StatusSort { get; set; }

        public IEnumerable<Task> Tasks { get; set; }

        public PagingInfo PagingInfo { get; set; }
    }
}
