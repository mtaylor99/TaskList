using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TaskList.DAL.DbContexts;
using TaskList.DAL.Interfaces;
using TaskList.DAL.Models;

namespace TaskList.DAL.Repositories
{
    public class TaskListRepository : ITaskListRepository
    {
        private ApplicationDbContext context;

        public TaskListRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<Task> GetTasks => context.Task
                                            .Include(l => l.Location)
                                            .Include(s => s.Status)
                                            .Include(u => u.User); //ThenInclude to a child of a child table

        public Task GetTask(int taskId)
        {
            return context.Task.FirstOrDefault(t => t.TaskId == taskId);
        }

        public int AddTask(Task task)
        {
            try
            {
                context.Task.Add(task);
                context.SaveChanges();

                return task.TaskId;
            }
            catch (Exception)
            {
                return -1;
            }  
        }

        public void EditTask(Task task)
        {
            if (task.TaskId > 0)
            {
                Task dbEntry = context.Task.FirstOrDefault(t => t.TaskId == task.TaskId);

                if (dbEntry != null)
                {
                    dbEntry.Description = task.Description;
                    dbEntry.LocationId = task.LocationId;
                    dbEntry.StatusId = task.StatusId;
                    dbEntry.UserId = task.UserId;
                    context.SaveChanges();
                }
            }
        }

        public Task DeleteTask(int taskId)
        {
            Task dbEntry = context.Task.FirstOrDefault(t => t.TaskId == taskId);

            if (dbEntry != null)
            {
                context.Task.Remove(dbEntry);
                context.SaveChanges();
            }

            return dbEntry;
        }
    }
}
