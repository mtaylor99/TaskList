using Microsoft.AspNetCore.Identity;

namespace TaskList.DAL.Models
{
    public class AspNetRole : IdentityRole
    {
        public string Description { get; set; }
    }
}
