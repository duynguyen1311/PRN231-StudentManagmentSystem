using AutoMapper;
using BusinessObject.Model;
using BusinessObject.Utility;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentManagingSystem_API.DTO;

namespace StudentManagingSystem_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IStudentRepository _repository;

        public UserController(UserManager<AppUser> userManager, IMapper mapper, IStudentRepository repository)
        {
            _userManager = userManager;
            _mapper = mapper;
            _repository = repository;
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> UserProfile(string id)
        {
            try
            {

                var user = await _userManager.FindByIdAsync(id);
                if (user == null) return NotFound("Can not find !");
                var map = _mapper.Map<UserProfileResponse>(user);
                if (User.IsInRole(RoleConstant.STUDENT))
                {
                    var st = await _repository.GetById(Guid.Parse(id));
                    map.StudentCode = st.StudentCode;
                }
                return Ok(map);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut("updateProfile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UserProfileRequest rq)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(rq.Id);
                _mapper.Map(rq, user);
                await _userManager.UpdateAsync(user);
                return Ok();
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
