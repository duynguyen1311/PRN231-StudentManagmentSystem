using BusinessObject.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagingSystem_Client.Services;
using System.Data;

namespace StudentManagingSystem_Client.Pages.PointPage
{
    [Authorize(Roles = RoleConstant.ADMIN + "," + RoleConstant.TEACHER)]
    public class DeletePointModel : PageModel
    {
        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var client = new ClientService(HttpContext);
            var res = await client.Delete("/api/Point/delete", $"?id={id}");
            return RedirectToPage("/PointPage/Point");
        }
    }
}
