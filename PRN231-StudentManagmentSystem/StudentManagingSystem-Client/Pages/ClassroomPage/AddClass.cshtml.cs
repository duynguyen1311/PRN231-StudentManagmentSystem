using AutoMapper;
using BusinessObject.Model;
using BusinessObject.Utility;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagingSystem_Client.Services;
using StudentManagingSystem_Client.ViewModel;
using System.Data;

namespace StudentManagingSystem_Client.Pages.ClassroomPage
{
    [Authorize(Roles = RoleConstant.ADMIN)]
    public class AddClassModel : PageModel
    {

        [BindProperty]
        public ClassRoomAddRequest ClassRoomAddRequest { get; set; }
        public List<Department> listDept { get; set; }
        public List<AppUser> listUser { get; set; }
        
        public async Task<IActionResult> OnGetAsync()
        {
            var client = new ClientService(HttpContext);
            listDept = await client.GetAll<List<Department>>("/api/Department/getall");
            listUser = await client.GetAll<List<AppUser>>("/api/Teacher/getall");
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var client = new ClientService(HttpContext);
            var res = await client.PostReturnResponse("/api/ClassRoom/add", ClassRoomAddRequest);
            if (!res.IsSuccessStatusCode)
            {
                var content = res.Content.ReadAsStringAsync().Result;
                if (content.Equals("Code is already existed !"))
                {
                    listDept = await client.GetAll<List<Department>>("/api/Department/getall");
                    listUser = await client.GetAll<List<AppUser>>("/api/Teacher/getall");
                    ViewData["messageCode"] = content;
                    return Page();
                }
            }
            return RedirectToPage("/ClassRoomPage/ClassRoom");
        }
    }
}
