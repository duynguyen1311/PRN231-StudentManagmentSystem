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

namespace StudentManagingSystem_Client.Pages.PointPage
{
    [Authorize(Roles = RoleConstant.ADMIN + "," + RoleConstant.TEACHER)]
    public class AddPointModel : PageModel
    {
        
        [BindProperty]
        public PointAddRequest PointAddRequest { get; set; }
        public List<Student> ListStudent { get; set; }
        public List<Subject> ListSubject { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var client = new ClientService(HttpContext);
            ListStudent = await client.GetAll<List<Student>>("/api/Student/getAllWithoutFilter");
            ListSubject = await client.GetAll<List<Subject>>("api/Subject/getall");
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var client = new ClientService(HttpContext);
            var res = await client.PostReturnResponse("/api/Point/add", PointAddRequest);
            if (!res.IsSuccessStatusCode)
            {
                var content = res.Content.ReadAsStringAsync().Result;
                if (content.Equals("Student already has point for this subject"))
                {
                    ListStudent = await client.GetAll<List<Student>>("/api/Student/getAllWithoutFilter");
                    ListSubject = await client.GetAll<List<Subject>>("api/Subject/getall");
                    ViewData["messageCode"] = content;
                    return Page();
                }
            }
            return RedirectToPage("/PointPage/Point");
        }
    }
}
