using BusinessObject.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OfficeOpenXml;
using StudentManagingSystem_Client.Services;
using StudentManagingSystem_Client.ViewModel;
using System.Globalization;

namespace StudentManagingSystem_Client.Pages.TeacherPage
{
    [Authorize(Roles = RoleConstant.ADMIN)]
    public class ImportTeacherModel : PageModel
    {
        public async Task<IActionResult> OnPostAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("file", "Please select a file.");
                return Page();
            }

            // Step 3: Parse the Excel file and create a list of objects
            var listTeacher = new List<TeacherAddRequest>();

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
                        var obj = new TeacherAddRequest()
                        {
                            FullName = worksheet.Cells[row, 1].Value?.ToString(),
                            Email = worksheet.Cells[row, 2].Value?.ToString(),
                            Adress = worksheet.Cells[row, 3].Value?.ToString(),
                            Phone = worksheet.Cells[row, 4].Value?.ToString(),
                            Gender = worksheet.Cells[row, 5].Value?.ToString(),
                            Password = "Abc@123",
                            Status = true,
                        };

                        listTeacher.Add(obj);
                    }
                }
            }

            var client = new ClientService(HttpContext);

            var res = await client.PostAdd("/api/Teacher/Import", listTeacher);
            return RedirectToPage("Teacher");
        }
    }
}
