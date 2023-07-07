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

namespace StudentManagingSystem_Client.Pages.PointPage
{
    [Authorize]
    public class PointModel : PageModel
    {
        public PagedList<PointResponse> ListPoint { get; set; }
        public List<Student> ListStudent { get; set; }
        public List<Subject> ListSubject { get; set; }
        [BindProperty]
        public string? Keyword { get; set; }
        [BindProperty]
        public int? Semester { get; set; }
        public int PageIndex { get; set; } = 1;
        public int TotalPage { get; set; }
        public string? SubjectId { get; set; }
        public string? StudentId { get; set; }
        public async Task<IActionResult> OnGetAsync(int? semester, string? keyword, Guid? stuId, Guid? subId, int pageIndex, int pagesize)
        {
            try
            {
                var client = new ClientService(HttpContext);
                var userid = HttpContext.User.FindFirstValue(ClaimTypes.Sid);
                var role = HttpContext.User.FindFirstValue(ClaimTypes.Role);
                ListSubject = await client.GetAll<List<Subject>>("api/Subject/getall");
                if (role.Contains(RoleConstant.STUDENT))
                {
                    ListStudent = new List<Student>();
                    Semester = semester;
                    Keyword = keyword;
                    if (pageIndex == 0) pageIndex = 1;
                    PageIndex = pageIndex;
                    pagesize = 4;

                    var st = await client.GetDetail<Student>("/api/Student/detail", $"?id={userid}");
                    if (st == null) throw new ArgumentException("Can not find!");
                    var requestModel = new PointSearchRequest
                    {
                        keyword = keyword,
                        semester = semester ?? st.InSemester,
                        page = pageIndex,
                        pagesize = pagesize,
                        studentId = Guid.Parse(userid),
                        subjectId = subId,
                    };

                    ListPoint = await client.PostSearch<PagedList<PointResponse>>("/api/Point/search", requestModel);
                    TotalPage = (int)(Math.Ceiling(ListPoint.TotalCount / (double)pagesize));
                }
                else
                {
                    ListStudent = await client.GetAll<List<Student>>("/api/Student/getAllWithoutFilter");
                    Keyword = keyword;
                    if (pageIndex == 0) pageIndex = 1;
                    PageIndex = pageIndex;
                    pagesize = 4;

                    var requestModel = new PointSearchRequest
                    {
                        keyword = keyword,
                        semester = semester,
                        page = pageIndex,
                        pagesize = pagesize,
                        studentId = stuId,
                        subjectId = subId,
                    };

                    ListPoint = await client.PostSearch<PagedList<PointResponse>>("/api/Point/search", requestModel);
                    TotalPage = (int)(Math.Ceiling(ListPoint.TotalCount / (double)pagesize));
                }

                return Page();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
