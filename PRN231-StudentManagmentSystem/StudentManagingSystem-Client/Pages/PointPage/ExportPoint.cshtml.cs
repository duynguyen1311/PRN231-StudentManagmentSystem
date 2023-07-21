using BusinessObject.Model;
using BusinessObject.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagingSystem_Client.Services;
using StudentManagingSystem_Client.ViewModel;
using System.Data;
using System.Security.Claims;

namespace StudentManagingSystem_Client.Pages.PointPage
{
    public class ExportPointModel : PageModel
    {
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
            var client = new ClientService(HttpContext);
            var userid = HttpContext.User.FindFirstValue(ClaimTypes.Sid);
            var role = HttpContext.User.FindFirstValue(ClaimTypes.Role);
            if (role.Contains(RoleConstant.STUDENT))
            {
                Semester = semester;
                Keyword = keyword;
                if (pageIndex == 0) pageIndex = 1;
                PageIndex = pageIndex;
                pagesize = 1000;
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

                var response = await client.Export("/api/Point/Export", requestModel);
                if (response.IsSuccessStatusCode)
                {
                    // Read the file content from the API response
                    byte[] fileBytes = await response.Content.ReadAsByteArrayAsync();

                    // Set the response headers for file download
                    var contentDisposition = new System.Net.Mime.ContentDisposition
                    {
                        FileName = "Point.xlsx", // Replace with the desired file name
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
            else if (role.Contains(RoleConstant.TEACHER))
            {


                Keyword = keyword;
                if (pageIndex == 0) pageIndex = 1;
                PageIndex = pageIndex;
                pagesize = 1000;
                Semester = semester;
                SubjectId = subjectId.ToString();
                ClassId = classId.ToString();
                var requestModel = new PointSearchRequest
                {
                    keyword = keyword,
                    semester = semester,
                    page = pageIndex,
                    pagesize = pagesize,
                    classId = classId,
                    subjectId = subjectId,
                };

                var response = await client.Export("/api/Point/Export", requestModel);
                if (response.IsSuccessStatusCode)
                {
                    // Read the file content from the API response
                    byte[] fileBytes = await response.Content.ReadAsByteArrayAsync();

                    // Set the response headers for file download
                    var contentDisposition = new System.Net.Mime.ContentDisposition
                    {
                        FileName = "Point.xlsx", // Replace with the desired file name
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
                if (pageIndex == 0) pageIndex = 1;
                PageIndex = pageIndex;
                pagesize = 1000;
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

                var response = await client.Export("/api/Point/Export", requestModel);
                if (response.IsSuccessStatusCode)
                {
                    // Read the file content from the API response
                    byte[] fileBytes = await response.Content.ReadAsByteArrayAsync();

                    // Set the response headers for file download
                    var contentDisposition = new System.Net.Mime.ContentDisposition
                    {
                        FileName = "Point.xlsx", // Replace with the desired file name
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
