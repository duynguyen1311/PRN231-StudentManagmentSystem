using BusinessObject.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OfficeOpenXml;
using StudentManagingSystem_Client.Services;

namespace StudentManagingSystem_Client.Pages.DepartmentPage
{
    public class ImportDepartmentModel : PageModel
    {
        public async Task<IActionResult> OnPostAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("file", "Please select a file.");
                return Page();
            }

            // Step 3: Parse the Excel file and create a list of objects
            var listDepartment = new List<Department>();

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
                        var obj = new Department
                        {
                            DepartmentCode = worksheet.Cells[row, 1].Value?.ToString(),
                            DepartmentName = worksheet.Cells[row, 2].Value?.ToString(),
                        };

                        listDepartment.Add(obj);
                    }
                }
            }

            var client = new ClientService(HttpContext);

            var res = await client.PostAdd("api/Department/Import", listDepartment);
            return RedirectToPage("Department");
        }
    }
}
