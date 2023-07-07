using BusinessObject.Model;
using BusinessObject.Utility;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagingSystem_Client.Services;
using StudentManagingSystem_Client.ViewModel;
using System.Data;

namespace StudentManagingSystem_Client.Pages.StudentPage
{
    [Authorize(Roles = RoleConstant.ADMIN)]
    public class StudentModel : PageModel
    {

        public PagedList<Student> ListStudent { get; set; }
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

            var requestModel = new StudentSearchRequest
            {
                keyword = keyword,
                status = status,
                page = pageIndex,
                pagesize = pagesize
            };

            ListStudent = await client.PostSearch<PagedList<Student>>("/api/Student/search", requestModel);
            TotalPage = (int)(Math.Ceiling(ListStudent.TotalCount / (double)pagesize));
            return Page();
        }

    }
}
