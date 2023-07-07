using BusinessObject.Model;
using BusinessObject.Utility;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagingSystem_Client.Services;
using StudentManagingSystem_Client.ViewModel;

namespace StudentManagingSystem_Client.Pages.DepartmentPage
{
    [Authorize]
    public class DepartmentModel : PageModel
    {

        public PagedList<Department> ListDepartment { get; set; }
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

            var requestModel = new DepartmentSearchRequest
            {
                keyword = keyword,
                status = status,
                page = pageIndex,
                pagesize = pagesize
            };

            ListDepartment = await client.PostSearch<PagedList<Department>>("/api/Department/search",requestModel);
            TotalPage = (int)(Math.Ceiling(ListDepartment.TotalCount / (double)pagesize));
            return Page();
        }

    }
}
