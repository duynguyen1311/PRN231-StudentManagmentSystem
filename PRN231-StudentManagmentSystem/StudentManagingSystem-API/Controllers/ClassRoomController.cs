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
    public class ClassRoomController : ControllerBase
    {
        private readonly IRoomRepository _repository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public ClassRoomController(IRoomRepository repository,IDepartmentRepository departmentRepository,IUserRepository userRepository, IMapper mapper, IConfiguration configuration)
        {
            _repository = repository;
            _departmentRepository = departmentRepository;
            _userRepository = userRepository;
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
        public async Task<IActionResult> Add([FromBody] ClassRoomAddRequest rq)
        {
            try
            {
                rq.CreatedDate = DateTime.Now;
                rq.CreatedBy = GetNameFromConext();
                var checkCode = await _repository.CheckAddExistCode(rq.ClassCode);
                if (!checkCode)
                {
                    return StatusCode(500, "Code is already existed !");
                }
                var map = _mapper.Map<ClassRoom>(rq);
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
        public async Task<IActionResult> Update([FromBody] ClassRoomUpdateRequest rq)
        {
            try
            {
                rq.LastModifiedDate = DateTime.Now;
                rq.LastModifiedBy = GetNameFromConext();
                var map = _mapper.Map<ClassRoom>(rq);
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
        public async Task<IActionResult> Search([FromBody] ClassRoomSearchRequest rq)
        {
            try
            {
                if (User.IsInRole(RoleConstant.TEACHER))
                {
                    var res = await _repository.Search(rq.keyword, rq.status, GetUserIdFromConext(), rq.page, rq.pagesize);
                    var map = _mapper.Map<PagedList<ClassRoomSearchResponse>>(res);
                    return Ok(map);
                }
                else
                {
                    var res = await _repository.Search(rq.keyword, rq.status, rq.teacherId, rq.page, rq.pagesize);
                    var map = _mapper.Map<PagedList<ClassRoomSearchResponse>>(res);
                    return Ok(map);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("listStudentByClass")]
        public async Task<IActionResult> ListStudentByClass([FromQuery] Guid Id)
        {
            try
            {
                var res = await _repository.ListStudentByClass(Id);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("searchClassByStudent")]
        public async Task<IActionResult> SearchClassByStudent([FromBody] ClassRoomSearchByStudentRequest rq)
        {
            try
            {
                rq.studentId = Guid.Parse(GetUserIdFromConext());
                var res = await _repository.SearchClassByStudent(rq.keyword, rq.status, rq.studentId, rq.page, rq.pagesize);
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
        public async Task<IActionResult> ImportFile([FromBody] List<ClassRoomImportRequest> rq)
        {
            try
            {
                foreach(var item in rq)
                {
                    var did = await _departmentRepository.GetIdByCode(item.DepartmentCode);
                    item.DepartmentId = did;
                    var tid = await _userRepository.GetIdByEmail(item.Email);
                    item.UserId = tid;
                }
                var map = _mapper.Map<List<ClassRoom>>(rq);
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
        public async Task<IActionResult> ExportFile(ClassRoomSearchRequest rq)
        {
            try
            {
                //var request = new List<Guid>();
                List<Guid>? request;
                if (User.IsInRole(RoleConstant.ADMIN))
                {
                    var rp2 = await _repository.Search(rq.keyword, rq.status, rq.teacherId, rq.page, rq.pagesize);
                    request = rp2.Data.Select(i => i.Id).ToList();
                }
                else if (User.IsInRole(RoleConstant.TEACHER))
                {
                    rq.teacherId = GetUserIdFromConext();
                    var rp2 = await _repository.Search(rq.keyword, rq.status, rq.teacherId, rq.page, rq.pagesize);
                    request = rp2.Data.Select(i => i.Id).ToList();
                }
                else
                {

                    rq.teacherId = GetUserIdFromConext();
                    var rp2 = await _repository.SearchClassByStudent(rq.keyword, rq.status, Guid.Parse(rq.teacherId), rq.page, rq.pagesize);
                    request = rp2.Data.Select(i => i.Id).ToList();
                }

                ExcelPackage.LicenseContext = LicenseContext.Commercial;
                var templatePath = _configuration.GetValue<string>("PathTemplate:classExport");
                var file = new FileStream(templatePath, FileMode.Open);
                using (ExcelPackage pkg = new ExcelPackage(file))
                {
                    var workSheet = pkg.Workbook.Worksheets["Class"];
                    var listHeader = new List<string> { "Id", "Class Code", "Class Name", "Department", "Teacher", "Status" };
                    for (int i = 0; i < listHeader.Count; i++)
                    {
                        workSheet.Cells[1, i + 1].Value = listHeader[i];
                        workSheet.Cells[1, i + 1].Style.Font.Bold = true;
                        workSheet.Cells[1, i + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    }


                    var listClassRoom = new List<ClassRoom>();
                    foreach (var item in request)
                    {
                        var temp3 = await _repository.GetById(item);
                        listClassRoom.Add(temp3);
                    }
                    var listCusTypeExport = listClassRoom.Select(i => new ClassRoomExport
                    {
                        Id = i.Id,
                        ClassCode = i.ClassCode,
                        ClassName = i.ClassName,
                        Department = (i.Department != null) ? i.Department.DepartmentName : null,
                        Teacher = (i.User != null) ? i.User.FullName : null,
                        Status = i.Status
                    }).ToList();
                    int row = 2;
                    for (int i = 0; i < listCusTypeExport.Count; i++)
                    {
                        workSheet.Cells[row, 1].Value = listCusTypeExport[i].Id;
                        workSheet.Cells[row, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        workSheet.Cells[row, 2].Value = listCusTypeExport[i].ClassCode;
                        workSheet.Cells[row, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        workSheet.Cells[row, 3].Value = listCusTypeExport[i].ClassName;
                        workSheet.Cells[row, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        workSheet.Cells[row, 4].Value = listCusTypeExport[i].Department;
                        workSheet.Cells[row, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        workSheet.Cells[row, 5].Value = listCusTypeExport[i].Teacher;
                        workSheet.Cells[row, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        workSheet.Cells[row, 6].Value = listCusTypeExport[i].Status == true ? "Hoạt động" : "Không hoạt động";
                        workSheet.Cells[row, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
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
                                $"ClassRoom_export.xlsx");
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
