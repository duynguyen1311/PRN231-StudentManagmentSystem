using BusinessObject.Model;
using BusinessObject.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagingSystem_Client.Services;
using StudentManagingSystem_Client.ViewModel;
using System.Security.Claims;

namespace StudentManagingSystem_Client.Pages.SubjectPage
{
    public class ExportSubjectModel : PageModel
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
                pagesize = 1000;

                var requestModel = new SubjectSearchByStudentRequest
                {
                    keyword = keyword,
                    status = status,
                    page = pageIndex,
                    pagesize = pagesize,
                    semester = semester,
                    studentId = Guid.Parse(userid)

                };
                var response = await client.Export("/api/Subject/Export", requestModel);
                if (response.IsSuccessStatusCode)
                {
                    // Read the file content from the API response
                    byte[] fileBytes = await response.Content.ReadAsByteArrayAsync();

                    // Set the response headers for file download
                    var contentDisposition = new System.Net.Mime.ContentDisposition
                    {
                        FileName = "Subject.xlsx", // Replace with the desired file name
                        Inline = false
                    };
                    Response.Headers.Add("Content-Disposition", contentDisposition.ToString());

                    // Return the file as a FileStreamResult
                    return new FileStreamResult(new MemoryStream(fileBytes), "application/octet-stream");
                }
                else
                {
                    // Handle the case when the API call fails
                    return Content("Failed to download the Excel file");
                }
            }
            else
            {
                Keyword = keyword;
                Status = status;
                Semester = semester;
                if (pageIndex == 0) pageIndex = 1;
                PageIndex = pageIndex;
                pagesize = 1000;

                var requestModel = new SubjectSearchRequest
                {
                    keyword = keyword,
                    status = status,
                    page = pageIndex,
                    pagesize = pagesize,
                    semester = semester
                };

                var response = await client.Export("/api/Subject/Export", requestModel);
                if (response.IsSuccessStatusCode)
                {
                    // Read the file content from the API response
                    byte[] fileBytes = await response.Content.ReadAsByteArrayAsync();

                    // Set the response headers for file download
                    var contentDisposition = new System.Net.Mime.ContentDisposition
                    {
                        FileName = "Subject.xlsx", // Replace with the desired file name
                        Inline = false
                    };
                    Response.Headers.Add("Content-Disposition", contentDisposition.ToString());

                    // Return the file as a FileStreamResult
                    return new FileStreamResult(new MemoryStream(fileBytes), "application/octet-stream");
                }
                else
                {
                    // Handle the case when the API call fails
                    return Content("Failed to download the Excel file");
                }
            }

        }
    }
}
