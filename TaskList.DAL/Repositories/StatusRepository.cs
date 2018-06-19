using System.Linq;
using TaskList.DAL.DbContexts;
using TaskList.DAL.Interfaces;
using TaskList.DAL.Models;

namespace TaskList.DAL.Repositories
{
    public class StatusRepository : IStatusRepository
    {
        private ApplicationDbContext context;

        public StatusRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<Status> GetStatuses => context.Status;

        public Status GetStatus(int statusId)
        {
            return context.Status.FirstOrDefault(s => s.StatusId == statusId);
        }

        public void AddStatus(Status status)
        {
            context.Status.Add(status);
            context.SaveChanges();
        }

        public void EditStatus(Status status)
        {
            if (status.StatusId > 0)
            {
                Status dbEntry = context.Status.FirstOrDefault(s => s.StatusId == status.StatusId);

                if (dbEntry != null)
                {
                    dbEntry.Name = status.Name;
                    context.SaveChanges();
                }
            }
        }

        public Status DeleteStatus(int statusId)
        {
            Status dbEntry = context.Status.FirstOrDefault(s => s.StatusId == statusId);

            if (dbEntry != null)
            {
                context.Status.Remove(dbEntry);
                context.SaveChanges();
            }

            return dbEntry;
        }
    }
}
