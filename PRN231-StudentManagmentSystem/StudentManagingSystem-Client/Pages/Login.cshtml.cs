using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagingSystem_Client.Services;
using StudentManagingSystem_Client.ViewModel;
using System.Security.Claims;

namespace StudentManagingSystem_Client.Pages
{
    public class LoginModel : PageModel
    {

        [BindProperty]
        public string Email { get; set; } = null!;

        [BindProperty]
        public string Password { get; set; } = null!;
        public IActionResult OnGet()
        {
            if (User.Identity?.IsAuthenticated == true) return RedirectToPage("/Index");
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (User.Identity?.IsAuthenticated == true) return RedirectToPage("/Index");

            var client = new ClientService(HttpContext);
            var requestModel = new LoginRequestModel { Email = Email, Password = Password };
            var res = await client.Post<LoginResponseModel>("api/UserJwt/login", requestModel);
            if (res == null)
            {
                ViewData["Title"] = "Wrong Email or Password!";
                return RedirectToPage("/Login", new { Error = "Login Failed!" });
            }
            HttpContext.Response.Cookies.Append("AccessToken", res.Token, new CookieOptions
            {
                Expires = DateTime.Now.AddDays(1).AddMinutes(-1)
            });
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, res.Id),
                new Claim(ClaimTypes.Name, res.FullName),
                new Claim(ClaimTypes.Email, res.Email),
                new Claim(ClaimTypes.Role, res.Role)
            };
            var claimsIdentity = new ClaimsIdentity(claims, "login");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync("CookieAuthentication", claimsPrincipal, new AuthenticationProperties
            {
                IsPersistent = false
            });
            return RedirectToPage("/Index");
        }
    }
}
