using BusinessObject.Model;
using BusinessObject.Utility;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagingSystem_Client.Services;

namespace StudentManagingSystem_Client.Pages.PointPage
{
    [Authorize(Roles = RoleConstant.ADMIN + "," + RoleConstant.TEACHER)]
    public class UpdatePointModel : PageModel
    {
       
        public List<Student> ListStudent { get; set; }
        public List<Subject> ListSubject { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; }

        [BindProperty]
        public Point Point { get; set; }
        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var client = new ClientService(HttpContext);
            ListStudent = await client.GetAll<List<Student>>("/api/Student/getAllWithoutFilter");
            ListSubject = await client.GetAll<List<Subject>>("api/Subject/getall");
            Point = await client.GetDetail<Point>("/api/Point/detail", $"?id={id}");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var client = new ClientService(HttpContext);
            var res = await client.Put("/api/Point/update", Point);
            return RedirectToPage("/PointPage/Point", new { pageIndex = PageIndex });
        }

    }
}
