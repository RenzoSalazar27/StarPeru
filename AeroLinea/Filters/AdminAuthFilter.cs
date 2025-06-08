using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AeroLinea.Filters
{
    public class AdminAuthFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var adminUser = context.HttpContext.Session.GetString("AdminUsername");
            if (string.IsNullOrEmpty(adminUser))
            {
                context.Result = new RedirectToActionResult("Login", "Admin", null);
            }
        }
    }
} 