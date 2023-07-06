using BusinessObject.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagingSystem_Client.Services;

namespace StudentManagingSystem_Client.Pages.NotificationPage
{
    [Authorize(Roles =RoleConstant.ADMIN)]
    public class DeleteNotificationModel : PageModel
    {
        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var client = new ClientService(HttpContext);
            var res = await client.Delete("/api/Notify/delete", $"?id={id}");
            return RedirectToPage("/NotificationPage/Notification");
        }
    }
}
