using BusinessObject.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagingSystem_Client.Services;
using System.Data;

namespace StudentManagingSystem_Client.Pages.StudentPage
{
    [Authorize(Roles = RoleConstant.ADMIN)]
    public class DeleteStudentModel : PageModel
    {
        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var client = new ClientService(HttpContext);
            var res = await client.Delete("/api/Student/delete", $"?id={id}");
            return RedirectToPage("/StudentPage/Student");
        }
    }
}
