using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskList.BL.Interaces;
using TaskList.DAL.Interfaces;
using TaskList.DAL.Models;

namespace TaskList.BL.Domain
{
    public class Identity : IIdentity
    {
        private ILogger logger;
        private IIdentityRepository identityRepository;

        public Identity(ILogger log, IIdentityRepository identityRepo)
        {
            logger = log;
            identityRepository = identityRepo;

            logger.LogInformation("Identity Business Logic - Constructor");
        }

        public IQueryable<AspNetGroup> GetAspNetGroups()
        {
            return identityRepository.GetAspNetGroups;
        }

        public IQueryable<AspNetGroup> GetAspNetGroups(int page, int pageSize)
        {
            logger.LogInformation("Identity Business Logic - GetAspNetGroups");

            return identityRepository.GetAspNetGroups
                .Include(gr => gr.Roles)
                .ThenInclude(r => r.Role)
                .OrderBy(g => g.GroupId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
        }

        public int GetAspNetGroupsCount()
        {
            logger.LogInformation("Identity Business Logic - GetAspNetGroupsCount");

            return identityRepository.GetAspNetGroups
                .Count();
        }

        public AspNetGroup GetAspNetGroup(int groupId)
        {
            logger.LogInformation("Identity Business Logic - GetAspNetGroup");

            return identityRepository.GetAspNetGroup(groupId);
        }

        public AspNetGroup GetAspNetGroup(string groupName)
        {
            logger.LogInformation("Identity Business Logic - GetAspNetGroup");

            return identityRepository.GetAspNetGroup(groupName);
        }

        public int AddGroup(AspNetGroup group, string[] selectedRoles)
        {
            logger.LogInformation("Identity Business Logic - AddGroup");

            var groupId = identityRepository.AddGroup(group);

            if (groupId > 0)
            {
                if (selectedRoles != null)
                {
                    selectedRoles = selectedRoles ?? new string[] { };

                    foreach (var role in selectedRoles)
                    {
                        AspNetRoleGroup roleGroup = new AspNetRoleGroup();
                        roleGroup.GroupId = groupId;
                        roleGroup.RoleId = role;
                        roleGroup.Allow = true;

                        identityRepository.AddRoleToGroup(roleGroup);
                    }
                }
            }

            return groupId;
        }

        public int EditGroup(AspNetGroup group, string[] selectedRoles)
        {
            logger.LogInformation("Identity Business Logic - EditGroup");

            var groupId = identityRepository.EditGroup(group);

            if (groupId > 0)
            {
                if (selectedRoles != null)
                {
                    selectedRoles = selectedRoles ?? new string[] { };

                    foreach (var role in selectedRoles)
                    {
                        AspNetRoleGroup roleGroup = new AspNetRoleGroup();
                        roleGroup.GroupId = groupId;
                        roleGroup.RoleId = role;
                        roleGroup.Allow = true;

                        identityRepository.AddRoleToGroup(roleGroup);
                    }
                }
            }

            return groupId;
        }

        public bool DeleteGroup(int groupId)
        {
            logger.LogInformation("Identity Business Logic - DeleteGroup");

            identityRepository.DeleteGroup(groupId);

            return true;
        }

        public IQueryable<AspNetUser> GetAspNetUsers()
        {
            logger.LogInformation("Identity Business Logic - GetAspNetUsers");

            return identityRepository.GetAspNetUsers
                .OrderBy(u => u.Email);
        }

        public IQueryable<AspNetUser> GetAspNetUsers(int page, int pageSize)
        {
            logger.LogInformation("Identity Business Logic - GetAspNetUsers");

            return identityRepository.GetAspNetUsers
                .OrderBy(u => u.Email)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
        }

        public int GetAspNetUsersCount()
        {
            logger.LogInformation("Identity Business Logic - GetAspNetUsersCount");

            return identityRepository.GetAspNetUsers
                .Count();
        }

        public AspNetUser GetAspNetUser(string aspNetUserId)
        {
            logger.LogInformation("Identity Business Logic - GetAspNetUser");

            return identityRepository.GetAspNetUser(aspNetUserId);
        }

        public string AddAspNetUser(AspNetUser aspNetUser, string password, string[] selectedGroups)
        {
            logger.LogInformation("Identity Business Logic - AddAspNetUser");

            var aspNetUserId = identityRepository.AddAspNetUser(aspNetUser, password);

            if (aspNetUserId != null)
            {
                if (selectedGroups != null)
                {
                    selectedGroups = selectedGroups ?? new string[] { };

                    foreach (var group in selectedGroups)
                    {
                        AspNetUserGroup userGroup = new AspNetUserGroup();
                        userGroup.UserId = aspNetUserId;
                        userGroup.GroupId = Convert.ToInt32(group);
                        userGroup.Active = true;

                        identityRepository.AddUserToGroup(userGroup);
                    }
                }
            }

            return aspNetUserId;
        }

        public string EditAspNetUser(AspNetUser aspNetUser, string password, string[] selectedGroups)
        {
            logger.LogInformation("Identity Business Logic - EditAspNetUser");

            var aspNetUserId = identityRepository.EditAspNetUser(aspNetUser, password);

            if (aspNetUserId != null)
            {

                //Delete exitins groups]
                identityRepository.DeleteUserGroups(aspNetUser.Id);

                if (selectedGroups != null)
                {
                    selectedGroups = selectedGroups ?? new string[] { };

                    foreach (var group in selectedGroups)
                    {
                        AspNetUserGroup userGroup = new AspNetUserGroup();
                        userGroup.UserId = aspNetUserId;
                        userGroup.GroupId = Convert.ToInt32(group);
                        userGroup.Active = true;

                        identityRepository.AddUserToGroup(userGroup);
                    }
                }
            }

            return aspNetUserId;
        }

        public bool DeleteAspNetUser(string aspNetUserId)
        {
            logger.LogInformation("Identity Business Logic - DeleteAspNetUser");

            identityRepository.DeleteAspNetUser(aspNetUserId);

            return true;
        }

        public IQueryable<AspNetRole> GetAspNetRoles()
        {
            logger.LogInformation("Identity Business Logic - GetAspNetRoles");

            return identityRepository.GetAspNetRoles;
        }

        public List<string> GetUserGroups(string aspNetUserId)
        {
            logger.LogInformation("Identity Business Logic - GetUserGroups");

            return identityRepository.GetUserGroups(aspNetUserId);
        }

        public List<string> GetRolesInGroup(int groupId)
        {
            logger.LogInformation("Identity Business Logic - AddRoleToGroup");

            return identityRepository.GetRolesInGroup(groupId);
        }

        public bool DeleteUserGroups(string aspNetUserId)
        {
            logger.LogInformation("Identity Business Logic - DeleteUserGroups");

            return identityRepository.DeleteUserGroups(aspNetUserId);
        }

        public bool AddRoleToGroup(AspNetRoleGroup aspNetRoleGroup)
        {
            logger.LogInformation("Identity Business Logic - AddRoleToGroup");

            identityRepository.AddRoleToGroup(aspNetRoleGroup);

            return true;
        }

        public bool AddUserToGroup(AspNetUserGroup aspNetUserGroup)
        {
            logger.LogInformation("Identity Business Logic - AddUserToGroup");

            identityRepository.AddUserToGroup(aspNetUserGroup);

            return true;
        }
    }
}
