using System.Linq;
using TaskList.DAL.Models;

namespace TaskList.BL.Interaces
{
    public interface IStatuses
    {
        IQueryable<Status> GetStatuses();

        IQueryable<Status> GetStatuses(int page, int pageSize);

        int GetStatusesCount();

        Status GetStatus(int statusId);

        bool AddStatus(Status status);

        bool EditStatus(Status status);

        bool DeleteStatus(int statusId);
    }
}
