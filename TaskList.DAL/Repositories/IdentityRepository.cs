using System.Linq;
using System.Collections.Generic;
using TaskList.DAL.DbContexts;
using TaskList.DAL.Models;
using TaskList.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace TaskList.DAL.Repositories
{
    public class IdentityRepository : IIdentityRepository
    {
        private ApplicationDbContext context;
        private UserManager<AspNetUser> userManager;

        public IdentityRepository(ApplicationDbContext ctx, UserManager<AspNetUser> usermanager)
        {
            context = ctx;
            userManager = usermanager;
        }

        public List<string> GetUserRoles(string userId)
        {
            List<string> result = new List<string>();

            var allowQuery = (
                from userGroups in context.AspNetUserGroup.Where(ug => ug.UserId == userId && ug.Active == true)
                join roleGroups in context.AspNetRoleGroup.Where(rg => rg.Allow == true) on userGroups.GroupId equals roleGroups.GroupId
                join roles in context.Roles on roleGroups.RoleId equals roles.Id

                select new
                {
                    Role = roles.Name

                }).ToList();

            var disallowQuery = (
                from userGroups in context.AspNetUserGroup.Where(ug => ug.UserId == userId && ug.Active == true)
                join roleGroups in context.AspNetRoleGroup.Where(rg => rg.Allow == false) on userGroups.GroupId equals roleGroups.GroupId
                join roles in context.Roles on roleGroups.RoleId equals roles.Id

                select new
                {
                    Role = roles.Name

                }).ToList();

            var query = allowQuery.Except(disallowQuery);

            foreach (var role in query)
            {
                result.Add(role.Role);
            }

            return result;
        }

        public IQueryable<AspNetGroup> GetAspNetGroups => context.AspNetGroup;

        public AspNetGroup GetAspNetGroup(int aspNetGroupId)
        {
            return context.AspNetGroup.FirstOrDefault(g => g.GroupId == aspNetGroupId);
        }

        public AspNetGroup GetAspNetGroup(string aspNetGroupName)
        {
            return context.AspNetGroup.FirstOrDefault(g => g.Name == aspNetGroupName);
        }

        public int AddGroup(AspNetGroup group)
        {
            context.AspNetGroup.Add(group);
            context.SaveChanges();

            return group.GroupId;
        }

        public int EditGroup(AspNetGroup group)
        {
            if (group.GroupId > 0)
            {
                AspNetGroup dbEntry = context.AspNetGroup
                    .Include(r => r.Roles)
                    .FirstOrDefault(g => g.GroupId == group.GroupId);

                if (dbEntry != null)
                {
                    dbEntry.Roles.Clear();
                    context.SaveChanges();

                    dbEntry.Name = group.Name;
                    dbEntry.Description = group.Description;
                    dbEntry.Active = group.Active;
                    context.SaveChanges();
                }

                return group.GroupId;
            }

            return 0;
        }

        public AspNetGroup DeleteGroup(int groupId)
        {
            AspNetGroup dbEntry = context.AspNetGroup
                  .Include(r => r.Roles)
                  .FirstOrDefault(g => g.GroupId == groupId);

            if (dbEntry != null)
            {
                context.AspNetGroup.Remove(dbEntry);
                context.SaveChanges();
            }

            return dbEntry;
        }

        public IQueryable<AspNetUser> GetAspNetUsers => context.Users;

        public AspNetUser GetAspNetUser(string aspNetUserId)
        {
            return context.Users.FirstOrDefault(u => u.Id == aspNetUserId);
        }

        public string AddAspNetUser(AspNetUser user, string password)
        {
            IdentityResult result = userManager.CreateAsync(user, password).Result;

            //if (result.Succeeded)
            //{
            //    userManager.AddToRoleAsync(user, "Admins");
            //}

            return user.Id;
        }

        public string EditAspNetUser(AspNetUser user, string password)
        {
            if (user.Id != null)
            {
                AspNetUser dbEntry = context.Users.FirstOrDefault(u => u.Id == user.Id);

                if (dbEntry != null)
                {
                    dbEntry.UserName = user.UserName;
                    dbEntry.FullName = user.FullName;
                    dbEntry.Email = user.Email;

                    if (password != string.Empty)
                    {
                        dbEntry.PasswordHash = userManager.PasswordHasher.HashPassword(user, password);
                    }

                    context.SaveChanges();
                }

                return user.Id;
            }

            return null;
        }

        public AspNetUser DeleteAspNetUser(string Id)
        {
            AspNetUser dbEntry = context.Users
                .FirstOrDefault(u => u.Id == Id);

            if (dbEntry != null)
            {
                context.Users.Remove(dbEntry);
                context.SaveChanges();
            }

            return dbEntry;
        }

        public IQueryable<AspNetRole> GetAspNetRoles => context.Roles;

        public List<string> GetUserGroups(string userId)
        {
            List<string> result = new List<string>();

            var query = (
                from users in context.Users.Where(u => u.Id == userId)
                join userGroups in context.AspNetUserGroup on users.Id equals userGroups.UserId
                join groups in context.AspNetGroup on userGroups.GroupId equals groups.GroupId

                select new
                {
                    groups.GroupId
                }).ToList();

            foreach (var group in query)
            {
                result.Add(group.GroupId.ToString());
            }

            return result;
        }

        public List<string> GetRolesInGroup(int groupId)
        {
            List<string> result = new List<string>();

            var query = (
                from groups in context.AspNetGroup.Where(g => g.GroupId == groupId)
                join roleGroups in context.AspNetRoleGroup on groups.GroupId equals roleGroups.GroupId
                join roles in context.Roles on roleGroups.RoleId equals roles.Id

                select new
                {
                    roles.Id
                }).ToList();

            foreach (var role in query)
            {
                result.Add(role.Id);
            }

            return result;
        }

        public bool DeleteUserGroups(string userId)
        {
            var userGroups = context.AspNetUserGroup.Where(u => u.UserId == userId).ToList();

            context.AspNetUserGroup.RemoveRange(userGroups);

            return true;
        }

        public void AddRoleToGroup(AspNetRoleGroup roleGroup)
        {
            context.AspNetRoleGroup.Add(roleGroup);
            context.SaveChanges();
        }

        public void AddUserToGroup(AspNetUserGroup userGroup)
        {
            context.AspNetUserGroup.Add(userGroup);
            context.SaveChanges();
        }
    }
}
