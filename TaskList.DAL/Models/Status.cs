using System.ComponentModel.DataAnnotations;

namespace TaskList.DAL.Models
{
    public class Status
    {
        public int StatusId { get; set; }

        [Required(ErrorMessage = "Please enter a status")]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
