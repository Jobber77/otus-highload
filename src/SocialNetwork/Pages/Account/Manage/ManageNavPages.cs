using System;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SocialNetwork.Pages.Account.Manage
{

    public static class ManageNavPages
    {
        public static string Index => "Index";

        public static string ChangePassword => "ChangePassword";

        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);

        public static string ChangePasswordNavClass(ViewContext viewContext) =>
            PageNavClass(viewContext, ChangePassword);

        public static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                             ?? Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }

        public static string IndexAriaCurrent(ViewContext viewContext) => AriaCurrent(viewContext, Index);

        public static string ChangePasswordAriaCurrent(ViewContext viewContext) =>
            AriaCurrent(viewContext, ChangePassword);

        public static string AriaCurrent(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                             ?? Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "page" : null;
        }
    }
}
