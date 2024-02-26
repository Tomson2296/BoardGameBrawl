#nullable disable
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BoardGameBrawl.Areas.Identity.Pages.Account.Admin
{
    public static class AdminWorkflowsPages
    {
        public static string Index => "Index";

        public static string Dashboard => "Dashboard";

        public static string ManageUsers => "ManageUsers";

        public static string Lockuots => "Lockouts";

        public static string Passwords => "Passwords";

        public static string CreateUser => "CreateUser";

        public static string UserRoles => "UserRoles";

        public static string IndexNavClass(ViewContext viewContext) => PageNavAdminClass(viewContext, Index);

        public static string DashboardNavClass(ViewContext viewContext) => PageNavAdminClass(viewContext, Dashboard);

        public static string ListUsersNavClass(ViewContext viewContext) => PageNavAdminClass(viewContext, ManageUsers);

        public static string LockoutsNavClass(ViewContext viewContext) => PageNavAdminClass(viewContext, Lockuots);

        public static string PasswordsNavClass(ViewContext viewContext) => PageNavAdminClass(viewContext, Passwords);

        public static string CreateUserNavClass(ViewContext viewContext) => PageNavAdminClass(viewContext, CreateUser);

        public static string UserRolesNavClass(ViewContext viewContext) => PageNavAdminClass(viewContext, UserRoles);

        public static string PageNavAdminClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}
