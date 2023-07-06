using AutoMapper;
using BusinessObject.Model;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagingSystem_Client.Services;

namespace StudentManagingSystem_Client.Pages.DepartmentPage
{
    [Authorize]
    public class UpdateDepartmentModel : PageModel
    {
        [BindProperty]
        public Department Department { get; set; }
        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; }
        
        public async Task<IActionResult> OnGetAsync(Guid id, int pageIndex)
        {
            var client = new ClientService(HttpContext);
            Department = await client.GetDetail<Department>("/api/Department/detail",$"?id={id}");
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var client = new ClientService(HttpContext);
            var res = await client.Put("/api/Department/update", Department);
            return RedirectToPage("/DepartmentPage/Department", new { pageIndex = PageIndex });
        }
    }
}
