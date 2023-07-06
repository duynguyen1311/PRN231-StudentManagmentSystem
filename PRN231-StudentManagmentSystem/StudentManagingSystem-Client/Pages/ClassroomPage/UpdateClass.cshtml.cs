using AutoMapper;
using BusinessObject.Model;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagingSystem_Client.Services;

namespace StudentManagingSystem_Client.Pages.ClassroomPage
{
    [Authorize]
    public class UpdateClassModel : PageModel
    {

        [BindProperty]
        public ClassRoom ClassRoom { get; set; }
        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; }
        public List<Department> listDept { get; set; }
        public List<AppUser> listUser { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var client = new ClientService(HttpContext);
            listDept = await client.GetAll<List<Department>>("/api/Department/getall");
            listUser = await client.GetAll<List<AppUser>>("/api/Teacher/getall");
            ClassRoom = await client.GetDetail<ClassRoom>("/api/ClassRoom/detail",$"?id={id}");
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var client = new ClientService(HttpContext);
            var res = await client.Put("/api/ClassRoom/update", ClassRoom);
            return RedirectToPage("/ClassRoomPage/ClassRoom", new { pageIndex = PageIndex });
        }
    }
}
