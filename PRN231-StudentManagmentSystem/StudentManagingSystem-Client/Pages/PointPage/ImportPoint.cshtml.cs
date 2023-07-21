using BusinessObject.Model;
using BusinessObject.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OfficeOpenXml;
using StudentManagingSystem_Client.Services;
using StudentManagingSystem_Client.ViewModel;
using System.Data;

namespace StudentManagingSystem_Client.Pages.PointPage
{
    [Authorize(Roles = RoleConstant.ADMIN)]
    public class ImportPointModel : PageModel
    {
        public async Task<IActionResult> OnPostAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("file", "Please select a file.");
                return Page();
            }

            // Step 3: Parse the Excel file and create a list of objects
            var listPoint = new List<PointImportRequest>();

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
                        var obj = new PointImportRequest
                        {
                            StudentCode = worksheet.Cells[row, 2].Value?.ToString(),
                            SubjectCode = worksheet.Cells[row, 4].Value?.ToString(),
                            ProgessPoint = worksheet.Cells[row, 5].Value?.ToString() == null ? 0 : float.Parse(worksheet.Cells[row, 5].Value?.ToString()),
                            MidtermPoint = worksheet.Cells[row, 6].Value?.ToString() == null ? 0 : float.Parse(worksheet.Cells[row, 6].Value?.ToString()),
                        };

                        listPoint.Add(obj);
                    }
                }
            }

            var client = new ClientService(HttpContext);

            var res = await client.PostAdd("api/Point/Import", listPoint);
            return RedirectToPage("Point");
        }
    }
}
