using AutoMapper;
using BusinessObject.Model;
using BusinessObject.Utility;
using DataAccess.Repository;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentManagingSystem_API.DTO;

namespace StudentManagingSystem_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentRepository _repository;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public StudentController(IStudentRepository repository, IMapper mapper, UserManager<AppUser> userManager)
        {
            _repository = repository;
            _mapper = mapper;
            _userManager = userManager;
        }


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
    }
}
