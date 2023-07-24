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
        public List<ClassRoomSearchResponse> ListClassRoom { get; set; }
        [BindProperty]
        public string? Keyword { get; set; }
        [BindProperty]
        public int? Semester { get; set; }
        public int PageIndex { get; set; } = 1;
        public int TotalPage { get; set; }
        [BindProperty]
        public string? SubjectId { get; set; }
        [BindProperty]
        public string? StudentId { get; set; }
        [BindProperty]
        public string? ClassId { get; set; }
        public async Task<IActionResult> OnGetAsync(int? semester, string? keyword, Guid? studentId, Guid? subjectId, Guid? classId, int pageIndex, int pagesize)
        {
            try
            {
                var client = new ClientService(HttpContext);
                var userid = HttpContext.User.FindFirstValue(ClaimTypes.Sid);
                var role = HttpContext.User.FindFirstValue(ClaimTypes.Role);
                ListSubject = await client.GetAll<List<Subject>>("api/Subject/getall");
                TempData["SelectedSubjectId"] = subjectId;
                TempData["SelectedStudentId"] = studentId;
                if (role.Contains(RoleConstant.STUDENT))
                {
                    ListStudent = new List<Student>();
                    Semester = semester;
                    Keyword = keyword;
                    if (pageIndex == 0) pageIndex = 1;
                    PageIndex = pageIndex;
                    pagesize = 5;

                    var st = await client.GetDetail<Student>("/api/Student/detail", $"?id={userid}");
                    if (st == null) throw new ArgumentException("Can not find!");
                    var requestModel = new PointSearchRequest
                    {
                        keyword = keyword,
                        semester = semester ?? st.InSemester,
                        page = pageIndex,
                        pagesize = pagesize,
                        studentId = Guid.Parse(userid),
                        subjectId = subjectId,
                    };

                    ListPoint = await client.PostSearch<PagedList<PointResponse>>("/api/Point/search", requestModel);
                    ViewData["message"] = null;
                    if (ListPoint.Data.Count() == 0 || ListPoint == null)
                    {
                        ViewData["mess"] = "There are not any mark for this semester yet !!!";
                        return Page();
                    }
                    TotalPage = (int)(Math.Ceiling(ListPoint.TotalCount / (double)pagesize));
                }
                else if (role.Contains(RoleConstant.TEACHER))
                {
                    ListStudent = new List<Student>();

                    var requestModel = new ClassRoomSearchRequest
                    {
                        page = 1,
                        pagesize = 5,
                        teacherId = userid
                    };

                    var list = await client.PostSearch<PagedList<ClassRoomSearchResponse>>("/api/ClassRoom/search", requestModel);
                    ListClassRoom = (List<ClassRoomSearchResponse>)list.Data;
                    ViewData["message"] = "Please select a class to view mark !";
                    if(classId == null) ListPoint = new PagedList<PointResponse>();
                    else
                    {
                        Keyword = keyword;
                        if (pageIndex == 0) pageIndex = 1;
                        PageIndex = pageIndex;
                        pagesize = 5;
                        Semester = semester;
                        SubjectId = subjectId.ToString();
                        ClassId = classId.ToString();
                        var requestModel2 = new PointSearchRequest
                        {
                            keyword = keyword,
                            semester = semester,
                            page = pageIndex,
                            pagesize = pagesize,
                            classId = classId,
                            subjectId = subjectId,
                        };

                        ListPoint = await client.PostSearch<PagedList<PointResponse>>("/api/Point/search", requestModel2);
                        ViewData["message"] = null;
                        if (ListPoint.Data.Count() == 0 || ListPoint == null)
                        {
                            ViewData["mess"] = "There are not any mark for this class yet !!!";
                            return Page();
                        }
                        TotalPage = (int)(Math.Ceiling(ListPoint.TotalCount / (double)pagesize));
                    }
                }
                else
                {
                    ListStudent = await client.GetAll<List<Student>>("/api/Student/getAllWithoutFilter");
                    Keyword = keyword;
                    if (pageIndex == 0) pageIndex = 1;
                    PageIndex = pageIndex;
                    pagesize = 5;
                    Semester = semester;
                    SubjectId = subjectId.ToString();
                    StudentId = studentId.ToString();
                    var requestModel = new PointSearchRequest
                    {
                        keyword = keyword,
                        semester = semester,
                        page = pageIndex,
                        pagesize = pagesize,
                        studentId = studentId,
                        subjectId = subjectId,
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
