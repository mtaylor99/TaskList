using System;
using System.Collections.Generic;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using TaskList.DAL.Interfaces;
using TaskList.DAL.Models;
using TaskList.Web.Infrastructure;

namespace TaskList.Web.Attribute.RequiresRole
{
    public class RoleProvider : IRoleProvider
    {
        private IIdentityRepository _repo;
        private IServiceProvider _services;
        private List<string> userRoles;

        public RoleProvider(IServiceProvider services, IIdentityRepository repo)
        {
            _services = services;
            _repo = repo;

            ISession session = _services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;

            if (session.GetJson<List<string>>("Roles") == null)
            {
                HttpContext context = _services.GetRequiredService<IHttpContextAccessor>()?.HttpContext;
                UserManager<AspNetUser> userManager = _services.GetRequiredService<UserManager<AspNetUser>>();

                var userId = userManager.GetUserId(context.User);

                if (userId != null)
                {
                    userRoles = _repo.GetUserRoles(userId);
                    session.SetJson("Roles", userRoles);
                }
            }
            else
            {
                userRoles = session.GetJson<List<string>>("Roles");
            }
        }

        public bool IsInRole(IPrincipal principal, string role)
        {
            if (userRoles.Contains(role))
                return true;
            else
                return false;
        }
    }
}
