using AutoMapper;
using BusinessObject.Model;
using BusinessObject.Utility;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagingSystem_Client.Services;
using StudentManagingSystem_Client.ViewModel;

namespace StudentManagingSystem_Client.Pages
{
    [Authorize]
    public class UserProfileModel : PageModel
    {


        [BindProperty]
        public UserProfileResponse Profile { get; set; }

        [BindProperty]
        public ChangePasswordRequest Request { get; set; }



        public async Task<IActionResult> OnGetAsync(string id)
        {
            var client = new ClientService(HttpContext);
            Profile = await client.GetDetail<UserProfileResponse>("/api/User/profile", $"?id={id}");
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var client = new ClientService(HttpContext);
                var res = await client.Put("/api/User/updateProfile", Profile);
                return Page();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToPage("Error");
            }
        }
        public async Task<IActionResult> OnPostChangePassword()
        {
            try
            {
                var client = new ClientService(HttpContext);
                var result = await client.PostReturnResponse("/api/User/changePassword", Request);
                if (result.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = result.Content.ReadAsStringAsync().Result;
                    return Page();
                }
                else
                {
                    TempData["ErrorMessage"] = result.Content.ReadAsStringAsync().Result;
                    return Page();
                }

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToPage("Error");
            }
        }

    }
}
