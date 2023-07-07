using BusinessObject.Model;
using BusinessObject.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OfficeOpenXml;
using StudentManagingSystem_Client.Services;
using StudentManagingSystem_Client.ViewModel;

namespace StudentManagingSystem_Client.Pages.SubjectPage
{
    [Authorize(Roles = RoleConstant.ADMIN)]
    public class ImportSubjectModel : PageModel
    {
        public async Task<IActionResult> OnPostAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("file", "Please select a file.");
                return Page();
            }

            // Step 3: Parse the Excel file and create a list of objects
            var listSubject = new List<SubjectAddRequest>();

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelPackage.LicenseContext = LicenseContext.Commercial;
                    var worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++) // Assuming the first row contains headers
                    {
                        var obj = new SubjectAddRequest()
                        {
                            SubjectCode = worksheet.Cells[row, 1].Value?.ToString(),
                            SubjectName = worksheet.Cells[row, 2].Value?.ToString(),
                            Description = worksheet.Cells[row, 3].Value?.ToString(),
                            Semester = int.Parse(worksheet.Cells[row, 4].Value?.ToString()),
                            Credit = int.Parse(worksheet.Cells[row, 5].Value?.ToString()),
                        };

                        listSubject.Add(obj);
                    }
                }
            }

            var client = new ClientService(HttpContext);

            var res = await client.PostAdd("api/Subject/Import", listSubject);
            return RedirectToPage("Subject");
        }
    }
}
