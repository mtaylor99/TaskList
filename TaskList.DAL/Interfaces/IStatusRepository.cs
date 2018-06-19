using System.Linq;
using TaskList.DAL.Models;

namespace TaskList.DAL.Interfaces
{
    public interface IStatusRepository
    {
        IQueryable<Status> GetStatuses { get; }

        Status GetStatus(int statusId);

        void AddStatus(Status status);

        void EditStatus(Status status);

        Status DeleteStatus(int statusId);
    }
}
