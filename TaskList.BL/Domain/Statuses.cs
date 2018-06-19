using System.Linq;
using Microsoft.Extensions.Logging;
using TaskList.BL.Interaces;
using TaskList.DAL.Interfaces;
using TaskList.DAL.Models;

namespace TaskList.BL.Domain
{
    public class Statuses : IStatuses
    {
        private ILogger logger;
        private IStatusRepository statusRepository;

        public Statuses(ILogger log, IStatusRepository statusRepo)
        {
            logger = log;
            statusRepository = statusRepo;

            logger.LogInformation("Statuses Business Logic - Constructor");
        }

        public IQueryable<Status> GetStatuses()
        {
            logger.LogInformation("Statuses Business Logic - GetStatuses");

            return statusRepository.GetStatuses;
        }

        public IQueryable<Status> GetStatuses (int page, int pageSize)
        {
            logger.LogInformation("Statuses Business Logic - GetStatuses");

            return statusRepository.GetStatuses
                .OrderBy(s => s.StatusId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
        }

        public int GetStatusesCount()
        {
            logger.LogInformation("Statuses Business Logic - GetStatusesCount");

            return statusRepository.GetStatuses
                .Count();
        }

        public Status GetStatus(int statusId)
        {
            logger.LogInformation("Statuses Business Logic - GetStatus");

            return statusRepository.GetStatus(statusId);
        }

        public bool AddStatus(Status status)
        {
            logger.LogInformation("Statuses Business Logic - AddStatus");

            statusRepository.AddStatus(status);

            return true;
        }

        public bool EditStatus(Status status)
        {
            logger.LogInformation("Statuses Business Logic - EditStatus");

            statusRepository.EditStatus(status);

            return true;
        }

        public bool DeleteStatus(int statusId)
        {
            logger.LogInformation("Statuses Business Logic - DeleteStatus");

            statusRepository.DeleteStatus(statusId);

            return true;
        }
    }
}
