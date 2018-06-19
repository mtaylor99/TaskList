using System;
using Microsoft.AspNetCore.Identity;

namespace TaskList.DAL.Models
{
    public class AspNetUser : IdentityUser
    {
        public string FullName { get; set; }

        public DateTime BirthDate { get; set; }
    }
}
