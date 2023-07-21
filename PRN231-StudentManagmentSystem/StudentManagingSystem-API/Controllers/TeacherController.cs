using AutoMapper;
using BusinessObject.Model;
using BusinessObject.Utility;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using StudentManagingSystem_API.DTO;

namespace StudentManagingSystem_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TeacherController : ControllerBase
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;

        public TeacherController(IUserRepository repository, IMapper mapper, UserManager<AppUser> userManager, IConfiguration configuration)
        {
            _repository = repository;
            _mapper = mapper;
            _userManager = userManager;
            _configuration = configuration;
        }

        [Authorize(Roles = RoleConstant.ADMIN)]
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] TeacherAddRequest rq)
        {
            try
            {
                rq.CreatedDate = DateTime.Now;
                var user = new AppUser()
                {
                    Id = Guid.NewGuid().ToString(),
                    FullName = rq.FullName,
                    Login = rq.Email,
                    Email = rq.Email,
                    UserName = rq.Email,
                    Adress = rq.Adress,
                    Phone = rq.Phone,
                    Gender = rq.Gender,
                    DOB = rq.DOB,
                    Type = 1,
                    Activated = rq.Status,
                    CreatedDate = rq.CreatedDate,
                    LastModifiedDate = null
                };
                var check = await _repository.CheckAddExistEmail(rq.Email);
                if (!check)
                {
                    return StatusCode(500, "Email is already existed !");
                }
                var res = await _userManager.CreateAsync(user, rq.Password);
                if (res.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, RoleConstant.TEACHER);
                }
                else
                {
                    throw new Exception("Something is wrong with Creating user !");
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
        public async Task<IActionResult> Update([FromBody] TeacherUpdateRequest rq)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(rq.Id);
                user = _mapper.Map(rq, user);
                user.LastModifiedDate = DateTime.Now;
                user.Type = 1;
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
        public async Task<IActionResult> Delete([FromQuery] string Id)
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
                var map = _mapper.Map<List<TeacherResponse>>(res);
                return Ok(map);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody] TeacherSearchRequest rq)
        {
            try
            {
                var res = await _repository.Search(rq.keyword, rq.status, rq.page, rq.pagesize);
                var map = _mapper.Map<PagedList<TeacherResponse>>(res);
                return Ok(map);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("detail")]
        public async Task<IActionResult> GetDetail([FromQuery] string Id)
        {
            try
            {
                var res = await _repository.GetById(Id);
                var map = _mapper.Map<TeacherResponse>(res);
                return Ok(map);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = RoleConstant.ADMIN)]
        [HttpPost("Import")]
        public async Task<IActionResult> ImportFile([FromBody] List<TeacherAddRequest> rq)
        {
            try
            {
                var map = _mapper.Map<List<AppUser>>(rq);
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
        public async Task<IActionResult> ExportFile(TeacherSearchRequest rq)
        {
            try
            {
                //var request = new List<Guid>();
                List<string>? request;
                var rp2 = await _repository.Search(rq.keyword, rq.status, rq.page, rq.pagesize);
                request = rp2.Data.Select(i => i.Id).ToList();

                ExcelPackage.LicenseContext = LicenseContext.Commercial;
                var templatePath = _configuration.GetValue<string>("PathTemplate:teacherExport");
                var file = new FileStream(templatePath, FileMode.Open);
                using (ExcelPackage pkg = new ExcelPackage(file))
                {
                    var workSheet = pkg.Workbook.Worksheets["Teacher"];
                    var listHeader = new List<string> { "Id", "Fullname", "Email", "Address", "Phone", "Gender", "DOB", "Status" };
                    for (int i = 0; i < listHeader.Count; i++)
                    {
                        workSheet.Cells[1, i + 1].Value = listHeader[i];
                        workSheet.Cells[1, i + 1].Style.Font.Bold = true;
                        workSheet.Cells[1, i + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    }


                    var listTeacher = new List<AppUser>();
                    foreach (var item in request)
                    {
                        var temp3 = await _repository.GetById(item);
                        listTeacher.Add(temp3);
                    }
                    var listCusTypeExport = listTeacher.Select(i => new TeacherResponse
                    {
                        Id = i.Id,
                        FullName = i.FullName,
                        Email = i.Email,
                        Adress = i.Adress,
                        Phone = i.Phone,
                        Gender = i.Gender,
                        DOB = i.DOB,
                        Status = i.Activated
                    }).ToList();
                    int row = 2;
                    for (int i = 0; i < listCusTypeExport.Count; i++)
                    {
                        workSheet.Cells[row, 1].Value = listCusTypeExport[i].Id;
                        workSheet.Cells[row, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        workSheet.Cells[row, 2].Value = listCusTypeExport[i].FullName;
                        workSheet.Cells[row, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        workSheet.Cells[row, 3].Value = listCusTypeExport[i].Email;
                        workSheet.Cells[row, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        workSheet.Cells[row, 4].Value = listCusTypeExport[i].Adress;
                        workSheet.Cells[row, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        workSheet.Cells[row, 5].Value = listCusTypeExport[i].Phone;
                        workSheet.Cells[row, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        workSheet.Cells[row, 6].Value = listCusTypeExport[i].Gender;
                        workSheet.Cells[row, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        workSheet.Cells[row, 7].Value = listCusTypeExport[i].DOB.Value.ToString("dd/MM/yyyy"); ;
                        workSheet.Cells[row, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        workSheet.Cells[row, 8].Value = listCusTypeExport[i].Status == true ? "Hoạt động" : "Không hoạt động";
                        workSheet.Cells[row, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
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
                                $"Teacher_export.xlsx");
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
