using System.Collections.Generic;
using TaskList.DAL.Models;

namespace TaskList.Web.Models.ViewModels
{
    public class AspNetUsersListViewModel
    {
        public IEnumerable<AspNetUser> AspNetUsers { get; set; }

        public PagingInfo PagingInfo { get; set; }
    }
}
