using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace TaskList.Web.Attribute.RequiresRole
{
    public class RequiresRoleAttribute : TypeFilterAttribute
    {
        public RequiresRoleAttribute(params string[] roles) : base(typeof(RequiresRoleAttributeExecutor))
        {
            Arguments = new[] { new RoleAuthorizationRequirement(roles) };
        }

        private class RequiresRoleAttributeExecutor : System.Attribute, IAsyncResourceFilter
        {
            private readonly ILogger _logger;
            private readonly RoleAuthorizationRequirement _requiredRoles;
            private readonly IRoleProvider _RoleProvider;

            public RequiresRoleAttributeExecutor(ILogger<RequiresRoleAttribute> logger,
                                            RoleAuthorizationRequirement requiredRoles,
                                            IRoleProvider roleProvider)
            {
                _logger = logger;
                _requiredRoles = requiredRoles;
                _RoleProvider = roleProvider;
            }

            public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
            {
                var principal = new AppPrincipal(_RoleProvider, context.HttpContext.User.Identity);
                bool isInOneOfThisRole = false;
                foreach (var item in _requiredRoles.RequiredRoles)
                {
                    if (principal.IsInRole(item))
                    {
                        isInOneOfThisRole = true;
                    }
                }

                if (isInOneOfThisRole == false)
                {
                    context.Result = new UnauthorizedResult();
                    await context.Result.ExecuteResultAsync(context);
                }
                else
                {
                    await next();
                }
            }
        }
    }
}
