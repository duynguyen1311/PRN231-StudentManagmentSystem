using AutoMapper;
using BusinessObject.Model;
using BusinessObject.Utility;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagingSystem_Client.Services;

namespace StudentManagingSystem_Client.Pages.SubjectPage
{
    [Authorize(Roles = RoleConstant.ADMIN + "," + RoleConstant.TEACHER)]
    public class UpdateSubjectModel : PageModel
    {

        [BindProperty]
        public Subject Subject { get; set; }
        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; }
        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var client = new ClientService(HttpContext);
            Subject = await client.GetDetail<Subject>("/api/Subject/detail", $"?id={id}");
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var client = new ClientService(HttpContext);
            var res = await client.Put("/api/Subject/update", Subject);
            return RedirectToPage("/SubjectPage/Subject", new { pageIndex = PageIndex });
        }
    }
}
