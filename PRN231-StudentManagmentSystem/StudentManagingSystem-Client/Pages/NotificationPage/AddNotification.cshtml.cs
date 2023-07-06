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
using System.Data;

namespace StudentManagingSystem_Client.Pages.NotificationPage
{
    [Authorize(Roles = RoleConstant.ADMIN)]
    public class AddNotificationModel : PageModel
    {

        [BindProperty]
        public NotifyAddRequest NotificationAddRequest { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var client = new ClientService(HttpContext);
                var res = await client.PostAdd("/api/Notify/add", NotificationAddRequest);
                return RedirectToPage("/NotificationPage/Notification");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToPage("/Error");
            }
        }
    }
}
