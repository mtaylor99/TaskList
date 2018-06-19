using System.Linq;
using Microsoft.Extensions.Logging;
using TaskList.BL.Interaces;
using TaskList.DAL.Interfaces;
using TaskList.DAL.Models;

namespace TaskList.BL.Domain
{
    public class Tasks : ITasks
    {
        private ILogger logger;
        private ITaskListRepository taskListRepository;

        public Tasks(ILogger log, ITaskListRepository taskListRepo)
        {
            logger = log;
            taskListRepository = taskListRepo;

            logger.LogInformation("Tasks Business Logic - Constructor");
        }

        public IQueryable<Task> GetTasks(string search, string sortBy, int page, int pageSize)
        {
            logger.LogInformation("Tasks Business Logic - GetTasks");

            var results = taskListRepository.GetTasks;

            if (!string.IsNullOrEmpty(search))
            {
                results = results.Where(t => t.Description.Contains(search) || t.Location.Description.Contains(search));
            }

            //sort the results
            switch (sortBy)
            {
                case "status":
                    results = results.OrderBy(t => t.Status.Name);
                    break;
                case "status_desc":
                    results = results.OrderByDescending(t => t.Status.Name);
                    break;
                case "description":
                    results = results.OrderBy(t => t.Description);
                    break;
                case "description_desc":
                default:
                    results = results.OrderByDescending(t => t.Description);
                    break;
            }

            return results
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
        }

        public int GetTasksCount(string search)
        {
            logger.LogInformation("Tasks Business Logic - GetTasksCount");

            var results = taskListRepository.GetTasks;

            if (!string.IsNullOrEmpty(search))
            {
                results = results.Where(t => t.Description.Contains(search));
            }

            return results.Count();
        }

        public Task GetTask(int taskId)
        {
            logger.LogInformation("Tasks Business Logic - GetTask");

            return taskListRepository.GetTask(taskId);
        }

        public bool AddTask(Task task)
        {
            logger.LogInformation("Tasks Business Logic - AddTask");

            taskListRepository.AddTask(task);

            return true;
        }

        public bool EditTask(Task task)
        {
            logger.LogInformation("Tasks Business Logic - EditTask");

            taskListRepository.EditTask(task);

            return true;
        }

        public bool DeleteTask(int taskId)
        {
            logger.LogInformation("Tasks Business Logic - DeleteTask");

            taskListRepository.DeleteTask(taskId);

            return true;
        }
    }
}
