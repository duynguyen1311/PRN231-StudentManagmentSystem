using AutoMapper;
using BusinessObject.Model;
using BusinessObject.Utility;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using StudentManagingSystem_API.DTO;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentManagingSystem_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserJwtController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;


        public UserJwtController(IUserRepository userRepository, IConfiguration configuration,
            IMapper mapper, UserManager<AppUser> userManager)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequestModel model)
        {
            var user = await LoadUserByUsername(model.Email);
            if (await _userManager.CheckPasswordAsync(user, model.Password))
            {
                if (!user.Activated) return StatusCode(500, "Not activated");
                var role = await _userManager.GetRolesAsync(user);

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JwtKey"));
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Issuer = _configuration["JwtIssuer"],
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Sid, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, $"{user.FullName}"),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Role, role.FirstOrDefault())
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenWrite = tokenHandler.WriteToken(token);
                var response = new LoginResponse();
                response.Token = tokenWrite;
                response.Role = role.FirstOrDefault();
                response.Id = user.Id;
                response.FullName = user.FullName;
                response.Email = user.Email;
                return Ok(response);
            }
            else
            {
                return Unauthorized(new { message = "Username or password is incorrect" });
            }

        }



        private async Task<AppUser> LoadUserByUsername(string email)
        {
            if (new EmailAddressAttribute().IsValid(email))
            {
                var userByEmail = await _userManager.FindByEmailAsync(email);
                if (userByEmail != null)
                    return userByEmail;
            }

            var lowerCaseLogin = email.ToLower(CultureInfo.GetCultureInfo("en-US"));
            var userByLogin = await _userManager.FindByNameAsync(email.ToLower());
            if (userByLogin == null)
                throw new UsernameNotFoundException($"User {lowerCaseLogin} was not found in the database");
            return userByLogin;
        }
    }
}
