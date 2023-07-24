using AutoMapper;
using BusinessObject.Model.Interface;
using BusinessObject.Model;
using BusinessObject.Utility;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using StudentManagingSystem_Client.ViewModel;
using StudentManagingSystem_Client.Services;

namespace StudentManagingSystem_Client.Pages.StudentPage
{
    [Authorize(Roles = RoleConstant.ADMIN)]
    public class AddStudentModel : PageModel
    {

        [BindProperty]
        public StudentAddRequest Request { get; set; }
        public List<ClassRoom> listClass { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var client = new ClientService(HttpContext);
            listClass = await client.GetAll<List<ClassRoom>>("/api/ClassRoom/getall");
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {

            var client = new ClientService(HttpContext);
            var res = await client.PostReturnResponse("/api/Student/add", Request);
            if (!res.IsSuccessStatusCode)
            {
                var content = res.Content.ReadAsStringAsync().Result;
                if (content.Equals("Email is already existed !"))
                {
                    listClass = await client.GetAll<List<ClassRoom>>("/api/ClassRoom/getall");
                    ViewData["message"] = content;
                    return Page();
                }
                if (content.Equals("Code is already existed !"))
                {
                    listClass = await client.GetAll<List<ClassRoom>>("/api/ClassRoom/getall");
                    ViewData["messageCode"] = content;
                    return Page();
                }
            }


            return RedirectToPage("/StudentPage/Student");
        }
    }
}
