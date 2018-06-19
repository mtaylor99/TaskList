using System.Security.Principal;

namespace TaskList.Web.Attribute.RequiresRole
{
    public interface IRoleProvider
    {
        bool IsInRole(IPrincipal principal, string role);
    }
}
