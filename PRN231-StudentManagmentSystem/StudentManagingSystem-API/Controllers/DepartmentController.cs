using AutoMapper;
using BusinessObject.Model;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using StudentManagingSystem_API.DTO;
using BusinessObject.Utility;

namespace StudentManagingSystem_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentRepository _repository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public DepartmentController(IDepartmentRepository repository, IMapper mapper,IConfiguration configuration)
        {
            _repository = repository;
            _mapper = mapper;
            _configuration = configuration;
        }

        [Authorize(Roles = RoleConstant.ADMIN)]
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] DepartmentAddRequest rq)
        {
            try
            {
                rq.CreatedDate = DateTime.Now;
                var checkCode = await _repository.CheckAddExistCode(rq.DepartmentCode);
                if (!checkCode)
                {
                    return StatusCode(500, "Code is already existed !");
                }
                var map = _mapper.Map<Department>(rq);
                await _repository.Add(map);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = RoleConstant.ADMIN)]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] DepartmentUpdateRequest rq)
        {
            try
            {
                rq.LastModifiedDate = DateTime.Now;
                var map = _mapper.Map<Department>(rq);
                await _repository.Update(map);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = RoleConstant.ADMIN)]
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] Guid Id)
        {
            try
            {
                await _repository.Delete(Id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = RoleConstant.ADMIN)]
        [HttpPost("deleteList")]
        public async Task<IActionResult> DeleteList(List<string> Id)
        {
            try
            {
                await _repository.DeleteList(Id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var res = await _repository.GetAll();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize]
        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody] DepartmentSearchRequest rq)
        {
            try
            {
                var res = await _repository.Search(rq.keyword, rq.status, rq.page, rq.pagesize);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize]
        [HttpGet("detail")]
        public async Task<IActionResult> GetDetail([FromQuery] Guid Id)
        {
            try
            {
                var res = await _repository.GetById(Id);
                if (res == null) throw new ArgumentException("Can not find !");
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = RoleConstant.ADMIN)]
        [HttpPost("Import")]
        public async Task<IActionResult> ImportFile([FromBody] List<Department> rq)
        {
            try
            {
                await _repository.Import(rq);
                return Ok();
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize]
        [HttpPost("Export")]
        public async Task<IActionResult> ExportFile(DepartmentSearchRequest rq)
        {
            try
            {
                //var request = new List<Guid>();
                List<Guid>? request;
                var rp2 = await _repository.Search(rq.keyword, rq.status, rq.page, rq.pagesize);
                request = rp2.Data.Select(i => i.Id).ToList();

                ExcelPackage.LicenseContext = LicenseContext.Commercial;
                var templatePath = _configuration.GetValue<string>("PathTemplate:departmentExport");
                var file = new FileStream(templatePath, FileMode.Open);
                using (ExcelPackage pkg = new ExcelPackage(file))
                {
                    var workSheet = pkg.Workbook.Worksheets["Department"];
                    var listHeader = new List<string> { "Id", "Department Code", "Department Name" , "Status"};
                    for (int i = 0; i < listHeader.Count; i++)
                    {
                        workSheet.Cells[1, i + 1].Value = listHeader[i];
                        workSheet.Cells[1, i + 1].Style.Font.Bold = true;
                        workSheet.Cells[1, i + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    }


                    var listDepartment = new List<Department>();
                    foreach (var item in request)
                    {
                        var temp3 = await _repository.GetById(item);
                        listDepartment.Add(temp3);
                    }
                    var listCusTypeExport = listDepartment.Select(i => new Department
                    {
                        Id = i.Id,
                        DepartmentCode = i.DepartmentCode,
                        DepartmentName = i.DepartmentName,
                        Status = i.Status
                    }).ToList();
                    int row = 2;
                    for (int i = 0; i < listCusTypeExport.Count; i++)
                    {
                        workSheet.Cells[row, 1].Value = listCusTypeExport[i].Id;
                        workSheet.Cells[row, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        workSheet.Cells[row, 2].Value = listCusTypeExport[i].DepartmentCode;
                        workSheet.Cells[row, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        workSheet.Cells[row, 3].Value = listCusTypeExport[i].DepartmentName;
                        workSheet.Cells[row, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        workSheet.Cells[row, 4].Value = listCusTypeExport[i].Status == true ? "Hoạt động" : "Không hoạt động";
                        workSheet.Cells[row, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        row++;
                    }
                    workSheet.Cells.AutoFitColumns();
                    workSheet.Column(1).Width = 36;
                    workSheet.Rows.Height = 25;
                    workSheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    pkg.Save();
                    try
                    {
                        using (var stream = new MemoryStream())
                        {
                            pkg.SaveAs(stream);
                            var content = stream.ToArray();
                            //string timestamp = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss").ToUpper().Remove('-').Remove(' ').Remove(':').Trim();
                            return File(
                                content,
                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                $"department_export.xlsx");
                        }

                    }
                    catch (Exception ex)
                    {
                        return StatusCode(500, ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
