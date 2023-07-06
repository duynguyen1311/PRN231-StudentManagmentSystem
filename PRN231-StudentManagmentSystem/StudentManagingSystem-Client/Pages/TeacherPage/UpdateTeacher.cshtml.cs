using AutoMapper;
using BusinessObject.Model;
using BusinessObject.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagingSystem_Client.Services;
using System.Data;

namespace StudentManagingSystem_Client.Pages.TeacherPage
{
    [Authorize(Roles = RoleConstant.ADMIN)]

    public class UpdateTeacherModel : PageModel
    {

        [BindProperty]
        public AppUser Teacher { get; set; }
        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; }
        public async Task<IActionResult> OnGetAsync(Guid id, int pageIndex)
        {
            var client = new ClientService(HttpContext);
            Teacher = await client.GetDetail<AppUser>("/api/Teacher/detail", $"?id={id}");
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var client = new ClientService(HttpContext);
            var res = await client.Put("/api/Teacher/update", Teacher);
            return RedirectToPage("/TeacherPage/Teacher", new { pageIndex = PageIndex });
        }
    }
}
