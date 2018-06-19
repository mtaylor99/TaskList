using System.Collections.Generic;
using System.Linq;
using TaskList.DAL.Models;

namespace TaskList.DAL.Interfaces
{
    public interface IIdentityRepository
    {
        List<string> GetUserRoles(string userId);

        IQueryable<AspNetGroup> GetAspNetGroups { get; }

        AspNetGroup GetAspNetGroup(int aspNetGroupId);

        AspNetGroup GetAspNetGroup(string aspNetGroupName);

        int AddGroup(AspNetGroup group);

        int EditGroup(AspNetGroup group);

        AspNetGroup DeleteGroup(int groupId);

        IQueryable<AspNetUser> GetAspNetUsers { get; }

        AspNetUser GetAspNetUser(string aspNetUserId);

        string AddAspNetUser(AspNetUser user, string password);

        string EditAspNetUser(AspNetUser user, string password);

        AspNetUser DeleteAspNetUser(string Id);

        IQueryable<AspNetRole> GetAspNetRoles { get; }

        List<string> GetUserGroups(string userId);

        List<string> GetRolesInGroup(int groupId);

        bool DeleteUserGroups(string Id);

        void AddRoleToGroup(AspNetRoleGroup roleGroup);

        void AddUserToGroup(AspNetUserGroup userGroup);
    }
}
