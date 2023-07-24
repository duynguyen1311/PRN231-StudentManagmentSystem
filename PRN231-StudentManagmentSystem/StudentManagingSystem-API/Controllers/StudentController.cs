using AutoMapper;
using BusinessObject.Model;
using BusinessObject.Utility;
using DataAccess.Repository;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using StudentManagingSystem_API.DTO;

namespace StudentManagingSystem_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StudentController : ControllerBase
    {
        private readonly IStudentRepository _repository;
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;

        public StudentController(IStudentRepository repository,IRoomRepository roomRepository, IMapper mapper, UserManager<AppUser> userManager, IConfiguration configuration)
        {
            _repository = repository;
            _roomRepository = roomRepository;
            _mapper = mapper;
            _userManager = userManager;
            _configuration = configuration;
        }

        [Authorize(Roles = RoleConstant.ADMIN)]
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] StudentAddRequest rq)
        {
            try
            {
                rq.CreatedDate = DateTime.Now;
                var map = _mapper.Map<Student>(rq);
                var check = await _repository.CheckAddExistEmail(rq.Email);
                if (!check)
                {
                    return StatusCode(500, "Email is already existed !");
                }
                var checkCode = await _repository.CheckAddExistCode(rq.StudentCode);
                if (!checkCode)
                {
                    return StatusCode(500, "Code is already existed !");
                }
                await _repository.Add(map);
                var user = new AppUser()
                {
                    Id = map.Id.ToString(),
                    FullName = map.StudentName,
                    Login = map.Email,
                    Email = map.Email,
                    UserName = map.Email,
                    Adress = map.Address,
                    Gender = map.Gender,
                    DOB = map.DOB,
                    Phone = map.Phone,
                    Type = 0,
                };
                if (map.Status == true)
                {
                    user.Activated = true;
                }
                else
                {
                    user.Activated = false;
                }
                var res = await _userManager.CreateAsync(user, rq.Password);
                if (res.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, RoleConstant.STUDENT);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = RoleConstant.ADMIN)]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] StudentUpdateRequest rq)
        {
            try
            {
                rq.LastModifiedDate = DateTime.Now;
                var map = _mapper.Map<Student>(rq);
                await _repository.Update(map);
                var user = await _userManager.FindByIdAsync(map.Id.ToString());
                if (map.Status == true)
                {
                    user.Activated = true;
                }
                else
                {
                    user.Activated = false;
                }
                await _userManager.UpdateAsync(user);
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
                var user = await _userManager.FindByIdAsync(Id.ToString());
                user.Activated = false;
                await _userManager.UpdateAsync(user);
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
                foreach (var item in Id)
                {
                    var user = await _userManager.FindByIdAsync(item);
                    user.Activated = false;
                    await _userManager.UpdateAsync(user);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody] StudentSearchRequest rq)
        {
            try
            {
                var res = await _repository.GetAll(rq.keyword, rq.status, rq.page, rq.pagesize);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getAllWithoutFilter")]
        public async Task<IActionResult> GetAllWithoutFilter()
        {
            try
            {
                var res = await _repository.GetAllWithoutFilter();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("getStudentByClass")]
        public async Task<IActionResult> GetStudentByClass([FromQuery] Guid? Id)
        {
            try
            {
                var res = await _repository.GetStudentByClass(Id);
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
        public async Task<IActionResult> ImportFile([FromBody] List<StudentAddRequest> rq)
        {
            try
            {
                foreach(var itemm in rq)
                {
                    var cid = await _roomRepository.GetIdByCode(itemm.ClassCode);
                    itemm.ClassRoomId = cid;
                }
                var map = _mapper.Map<List<Student>>(rq);
                await _repository.Import(map);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = RoleConstant.ADMIN + "," + RoleConstant.TEACHER)]
        [HttpPost("Export")]
        public async Task<IActionResult> ExportFile(StudentSearchRequest rq)
        {
            try
            {
                //var request = new List<Guid>();
                List<Guid>? request;
                var rp2 = await _repository.GetAll(rq.keyword, rq.status, rq.page, rq.pagesize);
                request = rp2.Data.Select(i => i.Id).ToList();

                ExcelPackage.LicenseContext = LicenseContext.Commercial;
                var templatePath = _configuration.GetValue<string>("PathTemplate:studentExport");
                var file = new FileStream(templatePath, FileMode.Open);
                using (ExcelPackage pkg = new ExcelPackage(file))
                {
                    var workSheet = pkg.Workbook.Worksheets["Student"];
                    var listHeader = new List<string> { "Id", "Student Code", "Student Name","Class","Address"
                        ,"Email","Gender","DOB","Phone","In Semester", "Status" };
                    for (int i = 0; i < listHeader.Count; i++)
                    {
                        workSheet.Cells[1, i + 1].Value = listHeader[i];
                        workSheet.Cells[1, i + 1].Style.Font.Bold = true;
                        workSheet.Cells[1, i + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    }


                    var listStudent = new List<Student>();
                    foreach (var item in request)
                    {
                        var temp3 = await _repository.GetById(item);
                        listStudent.Add(temp3);
                    }
                    var listCusTypeExport = listStudent.Select(i => new StudentExport
                    {
                        Id = i.Id,
                        StudentCode = i.StudentCode,
                        StudentName = i.StudentName,
                        ClassName = (i.ClassRoom != null) ? i.ClassRoom.ClassName : null,
                        Address = i.Address,
                        Email = i.Email,
                        Gender = i.Gender,
                        DOB = i.DOB,
                        Phone = i.Phone,
                        InSemester = i.InSemester,
                        Status = i.Status
                    }).ToList();
                    int row = 2;
                    for (int i = 0; i < listCusTypeExport.Count; i++)
                    {
                        workSheet.Cells[row, 1].Value = listCusTypeExport[i].Id;
                        workSheet.Cells[row, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        workSheet.Cells[row, 2].Value = listCusTypeExport[i].StudentCode;
                        workSheet.Cells[row, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        workSheet.Cells[row, 3].Value = listCusTypeExport[i].StudentName;
                        workSheet.Cells[row, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        workSheet.Cells[row, 4].Value = listCusTypeExport[i].ClassName;
                        workSheet.Cells[row, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        workSheet.Cells[row, 5].Value = listCusTypeExport[i].Address;
                        workSheet.Cells[row, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        workSheet.Cells[row, 6].Value = listCusTypeExport[i].Email;
                        workSheet.Cells[row, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        workSheet.Cells[row, 7].Value = listCusTypeExport[i].Gender;
                        workSheet.Cells[row, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        workSheet.Cells[row, 8].Value = listCusTypeExport[i].DOB.Value.ToString("dd/MM/yyyy");
                        workSheet.Cells[row, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        workSheet.Cells[row, 9].Value = listCusTypeExport[i].Phone;
                        workSheet.Cells[row, 9].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        workSheet.Cells[row, 10].Value = listCusTypeExport[i].InSemester;
                        workSheet.Cells[row, 10].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        workSheet.Cells[row, 11].Value = listCusTypeExport[i].Status == true ? "Hoạt động" : "Không hoạt động";
                        workSheet.Cells[row, 11].Style.Border.BorderAround(ExcelBorderStyle.Thin);
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
                                $"Student_export.xlsx");
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
