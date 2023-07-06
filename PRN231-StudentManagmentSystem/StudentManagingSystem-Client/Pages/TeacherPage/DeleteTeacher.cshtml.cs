using BusinessObject.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagingSystem_Client.Services;

namespace StudentManagingSystem_Client.Pages.TeacherPage
{
    [Authorize(Roles =RoleConstant.ADMIN)]
    public class DeleteTeacherModel : PageModel
    {
        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var client = new ClientService(HttpContext);
            var res = await client.Delete("/api/Teacher/delete", $"?id={id}");
            return RedirectToPage("/TeacherPage/Teacher");
        }
    }
}
