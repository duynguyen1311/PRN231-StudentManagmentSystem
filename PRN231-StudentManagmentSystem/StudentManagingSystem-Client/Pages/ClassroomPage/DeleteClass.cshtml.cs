using BusinessObject.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagingSystem_Client.Services;
using System.Data;

namespace StudentManagingSystem_Client.Pages.ClassroomPage
{
    [Authorize(Roles = RoleConstant.ADMIN)]
    public class DeleteClassModel : PageModel
    {
        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var client = new ClientService(HttpContext);
            var res = await client.Delete("/api/ClassRoom/delete", $"?id={id}");
            return RedirectToPage("/ClassRoomPage/ClassRoom");
        }
    }
}
