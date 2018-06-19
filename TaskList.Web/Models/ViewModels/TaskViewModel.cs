using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TaskList.DAL.Models;

namespace TaskList.Web.Models.ViewModels
{
    public class TaskViewModel
    {
        [Required]
        public Task Task { get; set; }

        public IEnumerable<SelectListItem> Statuses { get; set; }

        public IEnumerable<SelectListItem> Locations { get; set; }

        public IEnumerable<SelectListItem> Users { get; set; }
    }
}