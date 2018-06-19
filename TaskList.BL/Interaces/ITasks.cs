using System;
using System.Collections.Generic;
using System.Linq;
using TaskList.DAL.Models;

namespace TaskList.BL.Interaces
{
    public interface ITasks
    {
        IQueryable<Task> GetTasks(string search, string sortBy, int page, int pageSize);

        int GetTasksCount(string search);

        Task GetTask(int taskId);

        bool AddTask(Task task);

        bool EditTask(Task task);

        bool DeleteTask(int taskId);
    }
}
