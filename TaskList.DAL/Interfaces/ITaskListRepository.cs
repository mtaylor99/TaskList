using System;
using System.Collections.Generic;
using System.Linq;

using TaskList.DAL.Models;

namespace TaskList.DAL.Interfaces
{
    public interface ITaskListRepository
    {
        IQueryable<Task> GetTasks { get; }

        Task GetTask(int taskId);

        int AddTask(Task task);

        void EditTask(Task task);

        Task DeleteTask(int taskId);
    }
}
