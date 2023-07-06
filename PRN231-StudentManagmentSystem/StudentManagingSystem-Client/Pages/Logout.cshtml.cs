using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace StudentManagingSystem_Client.Pages
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            // Clear the authentication cookie
            HttpContext.SignOutAsync("CookieAuthentication");
            // Redirect to the login page or any other page after logout
            return RedirectToPage("/Login");
        }
    }
}
