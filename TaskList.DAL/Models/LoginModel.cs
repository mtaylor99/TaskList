using System.ComponentModel.DataAnnotations;

namespace TaskList.DAL.Models
{
    public class LoginModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [UIHint("password")]
        public string Password { get; set; }

        public string ReturnUrl { get; set; } = "/";

    }
}
