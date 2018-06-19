using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskList.DAL.Models
{
    public class Task
    {
        public int TaskId { get; set; }

        [Required(ErrorMessage = "Please enter a description")]
        public string Description { get; set; }

        [DisplayName("Status")]
        [Required(ErrorMessage = "Please select a status")]
        public int? StatusId { get; set; }

        [DisplayName("Location")]
        [Required(ErrorMessage = "Please select a location")]
        public int? LocationId { get; set; }

        [DisplayName("User")]
        [Required(ErrorMessage = "Please select a user")]
        [ForeignKey("Id")]
        public string UserId { get; set; }

        public virtual Status Status { get; set; }

        public virtual Location Location { get; set; }

        public virtual AspNetUser User { get; set; }
    }
}