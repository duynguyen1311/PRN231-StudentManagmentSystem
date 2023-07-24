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
using System.Security.Claims;
using BusinessObject.Utility;

namespace StudentManagingSystem_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectRepository _repository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public SubjectController(ISubjectRepository repository, IMapper mapper, IConfiguration configuration)
        {
            _repository = repository;
            _mapper = mapper;
            _configuration = configuration;
        }

        private string? GetUserIdFromConext()
        {
            var a = User.FindFirstValue(ClaimTypes.Sid);
            return a;
        }

        private string? GetNameFromConext()
        {
            return User.FindFirstValue(ClaimTypes.Name);
        }

        [Authorize(Roles = RoleConstant.ADMIN)]
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] SubjectAddRequest rq)
        {
            try
            {
                rq.CreatedDate = DateTime.Now;
                rq.CreatedBy = GetNameFromConext();
                var checkCode = await _repository.CheckAddExistCode(rq.SubjectCode);
                if (!checkCode)
                {
                    return StatusCode(500, "Code is already existed !");
                }
                var map = _mapper.Map<Subject>(rq);
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
        public async Task<IActionResult> Update([FromBody] SubjectUpdateRequest rq)
        {
            try
            {
                rq.LastModifiedDate = DateTime.Now;
                rq.LastModifiedBy = GetNameFromConext();
                var map = _mapper.Map<Subject>(rq);
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
        public async Task<IActionResult> Search([FromBody] SubjectSearchRequest rq)
        {
            try
            {
                var res = await _repository.Search(rq.keyword, rq.status, rq.semester, rq.page, rq.pagesize);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("searchByStudent")]
        public async Task<IActionResult> SearchByStudent([FromBody] SubjectSearchByStudentRequest rq)
        {
            try
            {
                var res = await _repository.SearchByStudent(rq.keyword, rq.status,rq.studentId, rq.semester, rq.page, rq.pagesize);
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
        public async Task<IActionResult> ImportFile([FromBody] List<SubjectAddRequest> rq)
        {
            try
            {
                var map = _mapper.Map<List<Subject>>(rq);
                await _repository.Import(map);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("Export")]
        public async Task<IActionResult> ExportFile(SubjectSearchRequest rq)
        {
            try
            {
                //var request = new List<Guid>();
                List<Guid>? request;
                if (User.IsInRole(RoleConstant.STUDENT))
                {
                    var studentId = Guid.Parse(GetUserIdFromConext());
                    var rp2 = await _repository.SearchByStudent(rq.keyword, rq.status, studentId, rq.semester, rq.page, rq.pagesize);
                    request = rp2.Data.Select(i => i.Id).ToList();
                }
                else
                {
                    var rp2 = await _repository.Search(rq.keyword, rq.status, rq.semester, rq.page, rq.pagesize);
                    request = rp2.Data.Select(i => i.Id).ToList();
                }
                

                ExcelPackage.LicenseContext = LicenseContext.Commercial;
                var templatePath = _configuration.GetValue<string>("PathTemplate:subjectExport");
                var file = new FileStream(templatePath, FileMode.Open);
                using (ExcelPackage pkg = new ExcelPackage(file))
                {
                    var workSheet = pkg.Workbook.Worksheets["Subject"];
                    var listHeader = new List<string> { "Id", "Subject Code", "Subject Name", "Status","Description","Semester" };
                    for (int i = 0; i < listHeader.Count; i++)
                    {
                        workSheet.Cells[1, i + 1].Value = listHeader[i];
                        workSheet.Cells[1, i + 1].Style.Font.Bold = true;
                        workSheet.Cells[1, i + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    }


                    var listSubject = new List<Subject>();
                    foreach (var item in request)
                    {
                        var temp3 = await _repository.GetById(item);
                        listSubject.Add(temp3);
                    }
                    var listCusTypeExport = listSubject.Select(i => new Subject
                    {
                        Id = i.Id,
                        SubjectCode = i.SubjectCode,
                        SubjectName = i.SubjectName,
                        Status = i.Status,
                        Description = i.Description,
                        Semester = i.Semester,
                    }).ToList();
                    int row = 2;
                    for (int i = 0; i < listCusTypeExport.Count; i++)
                    {
                        workSheet.Cells[row, 1].Value = listCusTypeExport[i].Id;
                        workSheet.Cells[row, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        workSheet.Cells[row, 2].Value = listCusTypeExport[i].SubjectCode;
                        workSheet.Cells[row, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        workSheet.Cells[row, 3].Value = listCusTypeExport[i].SubjectName;
                        workSheet.Cells[row, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        workSheet.Cells[row, 4].Value = listCusTypeExport[i].Status == true ? "Hoạt động" : "Không hoạt động";
                        workSheet.Cells[row, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        workSheet.Cells[row, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        workSheet.Cells[row, 5].Value = listCusTypeExport[i].Description;
                        workSheet.Cells[row, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        workSheet.Cells[row, 6].Value = listCusTypeExport[i].Semester;
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
                                $"Subject_export.xlsx");
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
