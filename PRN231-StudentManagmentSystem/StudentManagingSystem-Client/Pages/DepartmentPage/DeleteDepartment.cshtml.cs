using BusinessObject.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagingSystem_Client.Services;
using StudentManagingSystem_Client.ViewModel;

namespace StudentManagingSystem_Client.Pages.DepartmentPage
{
    [Authorize(Roles = RoleConstant.ADMIN)]
    public class DeleteDepartmentModel : PageModel
    {

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var client = new ClientService(HttpContext);
            var res = await client.Delete("/api/Department/delete", $"?id={id}");
            return RedirectToPage("/DepartmentPage/Department");
        }
    }
}
