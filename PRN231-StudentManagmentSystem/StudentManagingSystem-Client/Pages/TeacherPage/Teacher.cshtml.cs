using BusinessObject.Model;
using BusinessObject.Utility;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagingSystem_Client.Services;
using StudentManagingSystem_Client.ViewModel;
using System.Data;

namespace StudentManagingSystem_Client.Pages.TeacherPage
{
    [Authorize(Roles = RoleConstant.ADMIN)]
    public class TeacherModel : PageModel
    {

        public PagedList<TeacherResponse> ListTeacher { get; set; }
        [BindProperty]
        public string? Keyword { get; set; }
        [BindProperty]
        public bool? Status { get; set; }
        public int PageIndex { get; set; } = 1;
        public int TotalPage { get; set; }
        public async Task<IActionResult> OnGetAsync(string? keyword, bool? status, int pageIndex, int pagesize)
        {
            var client = new ClientService(HttpContext);
            Keyword = keyword;
            Status = status;
            if (pageIndex == 0) pageIndex = 1;
            PageIndex = pageIndex;
            pagesize = 5;

            var requestModel = new TeacherSearchRequest
            {
                keyword = keyword,
                status = status,
                page = pageIndex,
                pagesize = pagesize
            };

            ListTeacher = await client.PostSearch<PagedList<TeacherResponse>>("/api/Teacher/search", requestModel);
            TotalPage = (int)(Math.Ceiling(ListTeacher.TotalCount / (double)pagesize));
            return Page();
        }
    }
}
