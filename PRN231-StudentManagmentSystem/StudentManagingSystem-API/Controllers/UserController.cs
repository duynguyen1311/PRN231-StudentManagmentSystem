using AutoMapper;
using BusinessObject.Model;
using BusinessObject.Utility;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentManagingSystem_API.DTO;
using System.Security.Claims;

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

        [Authorize]
        [HttpPost("changePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest rq)
        {
            var user = await _userManager.FindByIdAsync(GetUserIdFromConext());
            var checkOldPass = await _userManager.CheckPasswordAsync(user, rq.OldPassword);
            if (checkOldPass)
            {
                if (rq.NewPassword == rq.ConfirmPassword)
                {
                    var result = await _userManager.ChangePasswordAsync(user, rq.OldPassword, rq.NewPassword);
                    return Ok("Changed password successfully !");
                }
                else
                {
                    return BadRequest("Password is not matched !");
                }
            }
            else
            {
                return BadRequest("Old password is not correct !");
            }
        }

        [AllowAnonymous]
        [HttpGet("detail")]
        public async Task<IActionResult> Detail(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null) return NotFound("Can not find !");
                return Ok(user.Activated);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
