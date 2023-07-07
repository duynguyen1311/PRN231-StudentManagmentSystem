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
        /*public async Task<IActionResult> OnPostChangePassword()
        {
            try
            {
                var user = await _userManager.FindByIdAsync(Profile.Id);
                var checkOldPass = await _userManager.CheckPasswordAsync(user, Request.OldPassword);
                if (checkOldPass)
                {
                    if (Request.NewPassword == Request.ConfirmPassword)
                    {
                        var result = await _userManager.ChangePasswordAsync(user, Request.OldPassword, Request.NewPassword);
                        if (result.Succeeded)
                        {
                            TempData["SuccessMessage"] = "Change password successfully";
                            return Page();
                        }
                        TempData["ErrorMessage"] = result.Errors.FirstOrDefault().Description;
                        return Page();
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "New password and confirm password are not the same";
                        return Page();
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Old password is not correct";
                    return Page();
                }


            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToPage("Error");
            }
        }*/

    }
}
