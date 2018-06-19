using System.Collections.Generic;
using System.Linq;
using TaskList.DAL.Models;

namespace TaskList.BL.Interaces
{
    public interface IIdentity
    {
        IQueryable<AspNetGroup> GetAspNetGroups();

        IQueryable<AspNetGroup> GetAspNetGroups(int page, int pageSize);

        int GetAspNetGroupsCount();

        AspNetGroup GetAspNetGroup(int groupId);

        AspNetGroup GetAspNetGroup(string groupName);

        int AddGroup(AspNetGroup group, string[] selectedRoles);

        int EditGroup(AspNetGroup group, string[] selectedRoles);

        bool DeleteGroup(int groupId);

        IQueryable<AspNetUser> GetAspNetUsers();

        IQueryable<AspNetUser> GetAspNetUsers(int page, int pageSize);

        int GetAspNetUsersCount();

        AspNetUser GetAspNetUser(string aspNetUserId);

        string AddAspNetUser(AspNetUser aspNetUser, string password, string[] selectedGroups);

        string EditAspNetUser(AspNetUser aspNetUser, string password, string[] selectedGroups);

        bool DeleteAspNetUser(string aspNetUserId);

        IQueryable<AspNetRole> GetAspNetRoles();

        List<string> GetUserGroups(string aspNetUserId);

        List<string> GetRolesInGroup(int groupId);

        bool DeleteUserGroups(string aspNetUserId);

        bool AddRoleToGroup(AspNetRoleGroup aspNetRoleGroup);

        bool AddUserToGroup(AspNetUserGroup aspNetUserGroup);
    }
}