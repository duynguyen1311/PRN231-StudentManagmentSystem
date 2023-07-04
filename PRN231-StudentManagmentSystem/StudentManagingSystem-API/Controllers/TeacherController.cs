using AutoMapper;
using BusinessObject.Model;
using BusinessObject.Utility;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentManagingSystem_API.DTO;

namespace StudentManagingSystem_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public TeacherController(IUserRepository repository, IMapper mapper, UserManager<AppUser> userManager)
        {
            _repository = repository;
            _mapper = mapper;
            _userManager = userManager;
        }


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
                    Activated = true,
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

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] string Id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(Id);
                await _userManager.DeleteAsync(user);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

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
    }
}
