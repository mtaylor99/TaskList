using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaskList.DAL.Models
{
    public class AspNetGroup
    {
        public int GroupId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public virtual ICollection<AspNetRoleGroup> Roles { get; set; }

        public virtual ICollection<AspNetUserGroup> Users { get; set; }

        public bool Active { get; set; }
    }
}
