using BusinessObject.Model;
using BusinessObject.Utility;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagingSystem_Client.Services;
using StudentManagingSystem_Client.ViewModel;
using System.Security.Claims;

namespace StudentManagingSystem_Client.Pages.SubjectPage
{
    [Authorize]
    public class SubjectModel : PageModel
    {

        public PagedList<Subject> ListSubject { get; set; }
        [BindProperty]
        public string? Keyword { get; set; }
        [BindProperty]
        public int? Semester { get; set; }
        [BindProperty]
        public bool? Status { get; set; }
        public int PageIndex { get; set; } = 1;
        public int TotalPage { get; set; }

        public async Task<IActionResult> OnGetAsync(string? keyword, bool? status, int? semester, int pageIndex, int pagesize)
        {
            var client = new ClientService(HttpContext);
            var userid = HttpContext.User.FindFirstValue(ClaimTypes.Sid);
            var role = HttpContext.User.FindFirstValue(ClaimTypes.Role);
            if (role.Contains(RoleConstant.STUDENT))
            {
                Keyword = keyword;
                Status = status;
                Semester = semester;
                if (pageIndex == 0) pageIndex = 1;
                PageIndex = pageIndex;
                pagesize = 4;

                var requestModel = new SubjectSearchByStudentRequest
                {
                    keyword = keyword,
                    status = status,
                    page = pageIndex,
                    pagesize = pagesize,
                    semester = semester,
                    studentId = Guid.Parse(userid)
                    
                };
                ListSubject = await client.PostSearch<PagedList<Subject>>("/api/Subject/searchByStudent", requestModel);
                TotalPage = (int)(Math.Ceiling(ListSubject.TotalCount / (double)pagesize));
            }
            else
            {
                Keyword = keyword;
                Status = status;
                Semester = semester;
                if (pageIndex == 0) pageIndex = 1;
                PageIndex = pageIndex;
                pagesize = 4;

                var requestModel = new SubjectSearchRequest
                {
                    keyword = keyword,
                    status = status,
                    page = pageIndex,
                    pagesize = pagesize,
                    semester = semester
                };

                ListSubject = await client.PostSearch<PagedList<Subject>>("/api/Subject/search", requestModel);
                TotalPage = (int)(Math.Ceiling(ListSubject.TotalCount / (double)pagesize));
            }

            return Page();
        }
    }
}
