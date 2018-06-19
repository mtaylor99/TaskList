using System.Security.Claims;
using System.Security.Principal;

namespace TaskList.Web.Attribute.RequiresRole
{
    public class AppPrincipal : ClaimsPrincipal
    {
        private readonly IRoleProvider _roleProvider;

        public AppPrincipal(IRoleProvider roleProvider, IIdentity ntIdentity) : base((ClaimsIdentity)ntIdentity)
        {
            _roleProvider = roleProvider;
        }

        public override bool IsInRole(string role)
        {
            return _roleProvider.IsInRole(this, role);
        }
    }
}
