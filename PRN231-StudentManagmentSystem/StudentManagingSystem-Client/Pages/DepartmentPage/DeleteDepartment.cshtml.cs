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
        [BindProperty]
        public List<string> SelectedItems { get; set; }
        public async Task<IActionResult> OnGetAsync(Guid id, List<string> selectedItems)
        {
            var client = new ClientService(HttpContext);
            if (SelectedItems != null && SelectedItems.Count > 0)
            {
                var requestModel = new DepartmentDeleteListRequest
                {
                    Id = SelectedItems
                };
                var res1 = await client.PostAdd("/api/Department/deleteList", requestModel);
                return RedirectToPage("/DepartmentPage/Department");
            }
            else
            {
                var res = await client.Delete("/api/Department/delete", $"?id={id}");
                return RedirectToPage("/DepartmentPage/Department");
            }
            
        }
    }
}
