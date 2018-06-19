namespace TaskList.DAL.Models
{
    public class AspNetRoleGroup
    {
        public int GroupId { get; set; }

        public string RoleId { get; set; }

        public bool Allow { get; set; }

        public AspNetRole Role { get; set; }
    }
}
