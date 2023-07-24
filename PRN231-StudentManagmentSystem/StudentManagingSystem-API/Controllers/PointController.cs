using AutoMapper;
using BusinessObject.Model;
using BusinessObject.Utility;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using StudentManagingSystem_API.DTO;
using System.Security.Claims;

namespace StudentManagingSystem_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PointController : ControllerBase
    {
        private readonly IPointRepository _repository;
        private readonly IStudentRepository _studentRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public PointController(IPointRepository repository, IStudentRepository studentRepository, ISubjectRepository subjectRepository, IMapper mapper, IConfiguration configuration)
        {
            _repository = repository;
            _studentRepository = studentRepository;
            _subjectRepository = subjectRepository;
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

        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] PointAddRequest rq)
        {
            try
            {
                rq.Id = Guid.NewGuid();
                rq.CreatedBy = GetNameFromConext();
                rq.CreatedDate = DateTime.Now;
                var checkCode = await _repository.CheckExistId(rq.StudentId, rq.SubjectId);
                if (!checkCode)
                {
                    return StatusCode(500, "Student already has point for this subject");
                }
                var map = _mapper.Map<Point>(rq);
                await _repository.Add(map);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] PointUpdateRequest rq)
        {
            try
            {
                rq.LastModifiedDate = DateTime.Now;
                rq.LastModifiedBy = GetNameFromConext();
                var map = _mapper.Map<Point>(rq);
                await _repository.Update(map);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize]
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

        [Authorize]
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

        [Authorize]
        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody] PointSearchRequest rq)
        {
            try
            {
                var res = await _repository.Search(rq.keyword, rq.semester, rq.subjectId, rq.studentId, rq.classId, rq.page, rq.pagesize);
                var map = _mapper.Map<PagedList<PointResponse>>(res);
                return Ok(map);
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
                var map = _mapper.Map<PointResponse>(res);
                return Ok(map);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = RoleConstant.ADMIN)]
        [HttpPost("Import")]
        public async Task<IActionResult> ImportFile([FromBody] List<PointImportRequest> rq)
        {
            try
            {
                List<Point> lp = new List<Point>();
                foreach (var item in rq)
                {
                    if (item.StudentCode != null && item.SubjectCode != null)
                    {
                        var st = await _studentRepository.GetIdByCode(item.StudentCode);
                        var sub = await _subjectRepository.GetIdByCode(item.SubjectCode);
                        var point = new Point()
                        {
                            SubjectId = sub,
                            StudentId = st,
                            ProgessPoint = item.ProgessPoint,
                            MidtermPoint = item.MidtermPoint,
                        };
                        lp.Add(point);
                    }
                }
                await _repository.Import(lp);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("Export")]
        public async Task<IActionResult> ExportFile(PointSearchRequest rq)
        {
            try
            {
                //var request = new List<Guid>();
                List<Guid>? request;
                if (User.IsInRole(RoleConstant.STUDENT))
                {
                    rq.studentId = Guid.Parse(GetUserIdFromConext());
                    var rp2 = await _repository.Search(rq.keyword, rq.semester, rq.subjectId, rq.studentId, rq.classId, rq.page, rq.pagesize);
                    request = rp2.Data.Select(i => i.Id).ToList();
                }
                else if (User.IsInRole(RoleConstant.TEACHER))
                {
                    var rp2 = await _repository.Search(rq.keyword, rq.semester, rq.subjectId, null, rq.classId, rq.page, rq.pagesize);
                    request = rp2.Data.Select(i => i.Id).ToList();
                }
                else
                {
                    var rp2 = await _repository.Search(rq.keyword, rq.semester, rq.subjectId, rq.studentId, rq.classId, rq.page, rq.pagesize);
                    request = rp2.Data.Select(i => i.Id).ToList();
                }

                ExcelPackage.LicenseContext = LicenseContext.Commercial;
                var templatePath = _configuration.GetValue<string>("PathTemplate:pointExport");
                var file = new FileStream(templatePath, FileMode.Open);
                using (ExcelPackage pkg = new ExcelPackage(file))
                {
                    var workSheet = pkg.Workbook.Worksheets["Point"];
                    var listHeader = new List<string> { "Id", "Student Name", "Student Code",
                        "Subject Name", "Subject Code", "Semester","Progress Point", "Midterm Point","Final Point","Result"};
                    for (int i = 0; i < listHeader.Count; i++)
                    {
                        workSheet.Cells[1, i + 1].Value = listHeader[i];
                        workSheet.Cells[1, i + 1].Style.Font.Bold = true;
                        workSheet.Cells[1, i + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    }


                    var listPoint = new List<Point>();
                    foreach (var item in request)
                    {
                        var temp3 = await _repository.GetById(item);
                        listPoint.Add(temp3);
                    }
                    var listCusTypeExport = listPoint.Select(i => new PointExport
                    {
                        Id = i.Id,
                        StudentName = (i.Student != null) ? i.Student.StudentName : null,
                        StudentCode = (i.Student != null) ? i.Student.StudentCode : null,
                        SubjectName = (i.Subject != null) ? i.Subject.SubjectName : null,
                        SubjectCode = (i.Subject != null) ? i.Subject.SubjectCode : null,
                        Semester = (i.Subject != null) ? i.Subject.Semester : null,
                        ProgessPoint = i.ProgessPoint,
                        MidtermPoint = i.MidtermPoint,
                        FinalPoint = i.FinalPoint,
                        Result = (i.IsPassed == true) ? "Passed" : "Not Passed",
                    }).ToList();
                    int row = 2;
                    for (int i = 0; i < listCusTypeExport.Count; i++)
                    {
                        workSheet.Cells[row, 1].Value = listCusTypeExport[i].Id;
                        workSheet.Cells[row, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        workSheet.Cells[row, 2].Value = listCusTypeExport[i].StudentName;
                        workSheet.Cells[row, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        workSheet.Cells[row, 3].Value = listCusTypeExport[i].StudentCode;
                        workSheet.Cells[row, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        workSheet.Cells[row, 4].Value = listCusTypeExport[i].SubjectName;
                        workSheet.Cells[row, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        workSheet.Cells[row, 5].Value = listCusTypeExport[i].SubjectCode;
                        workSheet.Cells[row, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        workSheet.Cells[row, 6].Value = listCusTypeExport[i].Semester;
                        workSheet.Cells[row, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        workSheet.Cells[row, 7].Value = listCusTypeExport[i].ProgessPoint;
                        workSheet.Cells[row, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        workSheet.Cells[row, 8].Value = listCusTypeExport[i].MidtermPoint;
                        workSheet.Cells[row, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        workSheet.Cells[row, 9].Value = listCusTypeExport[i].FinalPoint;
                        workSheet.Cells[row, 9].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        workSheet.Cells[row, 10].Value = listCusTypeExport[i].Result;
                        workSheet.Cells[row, 10].Style.Border.BorderAround(ExcelBorderStyle.Thin);

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
                            stream.Close();
                            return File(
                                content,
                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                $"Point_export.xlsx");
                        }

                    }
                    catch (Exception ex)
                    {
                        return StatusCode(500, ex.Message);
                    }
                    file.Close();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
