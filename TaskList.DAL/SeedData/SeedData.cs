using System.Linq;
using Microsoft.Extensions.Logging;
using TaskList.DAL.Models;
using TaskList.DAL.DbContexts;
using TaskList.DAL.Repositories;
using Microsoft.AspNetCore.Identity;

namespace TaskList.DAL.SeedData
{
    public class SeedData
    {
        private static ApplicationDbContext context;

        public static async System.Threading.Tasks.Task Populate(ApplicationDbContext ctx, UserManager<AspNetUser> userManager, RoleManager<AspNetRole> roleManager, ILogger<SeedData> logger)
        {
            context = ctx;
                
            context.Database.EnsureCreated();

            if (!(context.Roles.Any()))
            {
                await CreateAspNetRoles(roleManager);
            }

            if (!(context.AspNetGroup.Any()))
            {
                CreateAdministratorAspNetGroup(userManager, roleManager);
            }

            if (!(context.Users.Any()))
            {
                await CreateAdminAccount(userManager, roleManager);
            }

            if (!(context.Status.Any()))
            {
                CreateStatuses();
            }

            if (!(context.Location.Any()))
            {
                CreateLocations();
            }

            if (!(context.Task.Any()))
            {
                CreateTasks(userManager);
            }
        }

        private static async System.Threading.Tasks.Task CreateAspNetRoles(RoleManager<AspNetRole> roleManager)
        {
            AspNetRole aspNetAdministratorRole = new AspNetRole();
            aspNetAdministratorRole.Name = "Administrator";
            aspNetAdministratorRole.Description = "Administrator";
            await roleManager.CreateAsync(aspNetAdministratorRole);

            AspNetRole aspNetEditorRole = new AspNetRole();
            aspNetEditorRole.Name = "Editor";
            aspNetEditorRole.Description = "Editor";
            await roleManager.CreateAsync(aspNetEditorRole);

            AspNetRole aspNetViewerRole = new AspNetRole();
            aspNetViewerRole.Name = "Viewer";
            aspNetViewerRole.Description = "Viewer";
            await roleManager.CreateAsync(aspNetViewerRole);
        }

        private static void CreateAdministratorAspNetGroup(UserManager<AspNetUser> userManager, RoleManager<AspNetRole> roleManager)
        {
            IdentityRepository repo = new IdentityRepository(context, userManager);

            AspNetGroup aspNetGroup = new AspNetGroup();
            aspNetGroup.Name = "Administrator";
            aspNetGroup.Description = "Administrator";
            aspNetGroup.Active = true;
            var groupId = repo.AddGroup(aspNetGroup);

            AspNetRole adminRole = roleManager.FindByNameAsync("Administrator").Result;

            AspNetRoleGroup aspNetRoleGroup = new AspNetRoleGroup();
            aspNetRoleGroup.GroupId = groupId;
            aspNetRoleGroup.RoleId = adminRole.Id;
            aspNetRoleGroup.Allow = true;

            repo.AddRoleToGroup(aspNetRoleGroup);
        }


        private static async System.Threading.Tasks.Task CreateAdminAccount(UserManager<AspNetUser> userManager, RoleManager<AspNetRole> roleManager)
        {
            string username = "Administrator";
            string email = "Administrator@TaskList.com";
            string password = "Secret123$";

            if (await userManager.FindByNameAsync(username) == null)
            {
                AspNetUser user = new AspNetUser
                {
                    UserName = username,
                    FullName = username,
                    Email = email
                };

                IdentityResult result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    IdentityRepository indentityRepo = new IdentityRepository(context, userManager);

                    AspNetUser aspNetUser = await userManager.FindByNameAsync(username);
                    AspNetGroup aspNetGroup = indentityRepo.GetAspNetGroup("Administrator");

                    AspNetUserGroup aspNetUserGroup = new AspNetUserGroup();
                    aspNetUserGroup.GroupId = aspNetGroup.GroupId;
                    aspNetUserGroup.UserId = aspNetUser.Id;
                    aspNetUserGroup.Active = true;

                    indentityRepo.AddUserToGroup(aspNetUserGroup);
                }
            }
        }

        private static void CreateStatuses()
        {
            StatusRepository repo = new StatusRepository(context);

            Status open = new Status();
            open.Name = "Open";
            repo.AddStatus(open);

            Status completed = new Status();
            completed.Name = "Completed";
            repo.AddStatus(completed);
        }

        private static void CreateLocations()
        {
            LocationRepository repo = new LocationRepository(context);

            Location projectName = new Location();
            projectName.Description = "St. Teilo School";
            int locationId = repo.AddLocation(projectName);
            
            Location groundFloor = new Location();
            groundFloor.Description = "Ground Floor";
            groundFloor.ParentId = locationId;
            repo.AddLocation(groundFloor);

            Location firstFloor = new Location();
            firstFloor.Description = "First Floor";
            firstFloor.ParentId = locationId;
            repo.AddLocation(firstFloor);
        }

        private static void CreateTasks(UserManager<AspNetUser> userManager)
        {
            IdentityRepository indentityRepo = new IdentityRepository(context, userManager);
            var userId = indentityRepo.GetAspNetUsers.FirstOrDefault().Id;

            TaskListRepository taskRepo = new TaskListRepository(context);

            for (var i=0; i < 100; i++)
            {
                Task task = new Task();
                task.Description = "Task " + i;
                task.Location = new Location { LocationId = 2 };
                task.Status = new Status { StatusId = 1 };
                task.User = new AspNetUser { Id = userId };
                taskRepo.AddTask(task);
            }
        }
    }
}
