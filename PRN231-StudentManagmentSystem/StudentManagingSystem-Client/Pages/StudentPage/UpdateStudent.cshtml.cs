using AutoMapper;
using BusinessObject.Model;
using BusinessObject.Utility;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagingSystem_Client.Services;
using System.Data;

namespace StudentManagingSystem_Client.Pages.StudentPage
{
    [Authorize(Roles = RoleConstant.ADMIN)]
    public class UpdateStudentModel : PageModel
    {
        
        [BindProperty]
        public Student Student { get; set; }
        public List<ClassRoom> listClass { get; set; }
        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; }
       
        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var client = new ClientService(HttpContext);
            listClass = await client.GetAll<List<ClassRoom>>("/api/ClassRoom/getall");
            Student = await client.GetDetail<Student>("/api/Student/detail", $"?id={id}");
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var client = new ClientService(HttpContext);
            var res = await client.Put("/api/Student/update", Student);
            return RedirectToPage("/StudentPage/Student", new { pageIndex = PageIndex });
        }
    }
}
