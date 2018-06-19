using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace TaskList.Web.Attribute.RequiresRole
{
    public class RoleAuthorizationRequirement : IAuthorizationRequirement
    {
        public IEnumerable<string> RequiredRoles { get; }

        public RoleAuthorizationRequirement(IEnumerable<string> requiredRoles)
        {
            RequiredRoles = requiredRoles;
        }
    }
}
