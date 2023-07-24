using BusinessObject.Model;
using BusinessObject.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OfficeOpenXml;
using StudentManagingSystem_Client.Services;
using StudentManagingSystem_Client.ViewModel;
using System.Globalization;

namespace StudentManagingSystem_Client.Pages.StudentPage
{
    [Authorize(Roles = RoleConstant.ADMIN)]
    public class ImportStudentModel : PageModel
    {
        public async Task<IActionResult> OnPostAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("file", "Please select a file.");
                return Page();
            }

            // Step 3: Parse the Excel file and create a list of objects
            var listStudent = new List<StudentAddRequest>();

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
                        var obj = new StudentAddRequest()
                        {
                            StudentCode = worksheet.Cells[row, 1].Value?.ToString(),
                            StudentName = worksheet.Cells[row, 2].Value?.ToString(),
                            Address = worksheet.Cells[row, 3].Value?.ToString(),
                            Email = worksheet.Cells[row, 4].Value?.ToString(),
                            Gender = worksheet.Cells[row, 5].Value?.ToString(),
                            DOB = DateTime.ParseExact(worksheet.Cells[row, 6].Value?.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            Phone = worksheet.Cells[row, 7].Value?.ToString(),
                            InSemester = int.Parse(worksheet.Cells[row, 8].Value?.ToString()),
                            ClassCode = worksheet.Cells[row, 9].Value?.ToString(),
                            Password = "Abc@123",
                            Status = true,
                        };

                        listStudent.Add(obj);
                    }
                }
            }

            var client = new ClientService(HttpContext);

            var res = await client.PostAdd("/api/Student/Import", listStudent);
            return RedirectToPage("Student");
        }
    }
}
