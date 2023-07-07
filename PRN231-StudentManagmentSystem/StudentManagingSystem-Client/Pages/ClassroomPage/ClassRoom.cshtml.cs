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

namespace StudentManagingSystem_Client.Pages.ClassroomPage
{
    [Authorize]
    public class ClassRoomModel : PageModel
    {

        public PagedList<ClassRoomSearchResponse> ListClassRoom { get; set; }
        public List<Student> ListStudent { get; set; }
        [BindProperty]
        public string? Keyword { get; set; }
        [BindProperty]
        public bool? Status { get; set; }
        public int PageIndex { get; set; } = 1;
        public int TotalPage { get; set; }
        
        public async Task<IActionResult> OnGetAsync(string? keyword, bool? status, int pageIndex, int pagesize)
        {
            var client = new ClientService(HttpContext);
            var userid = HttpContext.User.FindFirstValue(ClaimTypes.Sid);
            var role = HttpContext.User.FindFirstValue(ClaimTypes.Role);
            if (role.Contains(RoleConstant.TEACHER))
            {
                Keyword = keyword;
                Status = status;
                if (pageIndex == 0) pageIndex = 1;
                PageIndex = pageIndex;
                pagesize = 5;

                var requestModel = new ClassRoomSearchRequest
                {
                    keyword = keyword,
                    status = status,
                    page = pageIndex,
                    pagesize = pagesize,
                    teacherId = userid
                };

                ListClassRoom = await client.PostSearch<PagedList<ClassRoomSearchResponse>>("/api/ClassRoom/search", requestModel);
                TotalPage = (int)(Math.Ceiling(ListClassRoom.TotalCount / (double)pagesize));
            }
            else if (role.Contains(RoleConstant.STUDENT))
            {
                Keyword = keyword;
                Status = status;
                if (pageIndex == 0) pageIndex = 1;
                PageIndex = pageIndex;
                pagesize = 5;

                var requestModel = new ClassRoomSearchByStudentRequest
                {
                    keyword = keyword,
                    status = status,
                    page = pageIndex,
                    pagesize = pagesize,
                    studentId = Guid.Parse(userid)
                };

                ListClassRoom = await client.PostSearch<PagedList<ClassRoomSearchResponse>>("/api/ClassRoom/searchClassByStudent", requestModel);
                TotalPage = (int)(Math.Ceiling(ListClassRoom.TotalCount / (double)pagesize));
            }
            else
            {
                Keyword = keyword;
                Status = status;
                if (pageIndex == 0) pageIndex = 1;
                PageIndex = pageIndex;
                pagesize = 5;

                var requestModel = new ClassRoomSearchRequest
                {
                    keyword = keyword,
                    status = status,
                    page = pageIndex,
                    pagesize = pagesize,
                };

                ListClassRoom = await client.PostSearch<PagedList<ClassRoomSearchResponse>>("/api/ClassRoom/search", requestModel);
                TotalPage = (int)(Math.Ceiling(ListClassRoom.TotalCount / (double)pagesize));
            }

            return Page();
        }
    }
}
