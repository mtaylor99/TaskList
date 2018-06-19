namespace TaskList.DAL.Models
{
    public class Location
    {
        public int LocationId { get; set; }

        public int ParentId { get; set; }

        public string Description { get; set; }
    }
}
