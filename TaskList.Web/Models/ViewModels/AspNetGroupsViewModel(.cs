using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using TaskList.DAL.Models;

namespace TaskList.Web.Models.ViewModels
{
    public class AspNetGroupsViewModel
    {
        public AspNetGroupsViewModel()
        {
            this.AspNetRolesList = new List<SelectListItem>();
        }

        public AspNetGroup AspNetGroup { get; set; }

        public ICollection<SelectListItem> AspNetRolesList { get; set; }
    }
}
