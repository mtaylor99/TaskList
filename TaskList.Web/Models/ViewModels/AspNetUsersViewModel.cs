using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using TaskList.DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace TaskList.Web.Models.ViewModels
{
    public class AspNetUsersViewModel
    {
        public AspNetUsersViewModel()
        {
            this.AspNetGroupsList = new List<SelectListItem>();
        }

        public string Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public ICollection<SelectListItem> AspNetGroupsList { get; set; }
    }
}
