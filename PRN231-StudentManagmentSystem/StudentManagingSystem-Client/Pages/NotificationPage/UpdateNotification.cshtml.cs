using AutoMapper;
using BusinessObject.Model;
using BusinessObject.Utility;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagingSystem_Client.Services;
using System.Data;

namespace StudentManagingSystem_Client.Pages.NotificationPage
{
    [Authorize(Roles = RoleConstant.ADMIN)]
    public class UpdateNotificationModel : PageModel
    {

        [BindProperty]
        public Notification Notification { get; set; }
        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; }
        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var client = new ClientService(HttpContext);
            Notification = await client.GetDetail<Notification>("/api/Notify/detail", $"?id={id}");
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var client = new ClientService(HttpContext);
            var res = await client.Put("/api/Notify/update", Notification);
            return RedirectToPage("/NotificationPage/Notification", new { pageIndex = PageIndex });
        }
    }
}
