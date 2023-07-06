/*using AutoMapper;
using BusinessObject.Model;
using BusinessObject.Utility;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace StudentManagingSystem_Client.Pages
{
    [Authorize]
    public class UserProfileModel : PageModel
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        [BindProperty]
        public UserProfileViewModel Profile { get; set; }

        [BindProperty]
        public ChangePasswordRequest Request { get; set; }
        public UserProfileModel(IStudentRepository studentRepository, IUserRepository userRepository, UserManager<AppUser> userManager, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _userRepository = userRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var role = await _userManager.GetRolesAsync(user);
            if ((role.Contains(RoleConstant.ADMIN)) || (role.Contains(RoleConstant.TEACHER)))
            {
                Profile = _mapper.Map<UserProfileViewModel>(user);
                return Page();
            }
            var student = await _studentRepository.GetById(Guid.Parse(id));
            Profile = _mapper.Map<UserProfileViewModel>(user);
            Profile.StudentCode = student.StudentCode;
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var user = await _userManager.FindByIdAsync(Profile.Id);
                var role = await _userManager.GetRolesAsync(user);
                if ((role.Contains(RoleConstant.ADMIN)) || (role.Contains(RoleConstant.TEACHER)))
                {
                    _mapper.Map(Profile, user);
                    await _userManager.UpdateAsync(user);
                    return Page();
                }
                var student = await _studentRepository.GetById(Guid.Parse(Profile.Id));
                Profile.StudentCode = student.StudentCode;
                _mapper.Map(Profile, user);
                _mapper.Map(Profile, student);
                await _userManager.UpdateAsync(user);
                await _studentRepository.Update(student);
                return Page();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToPage("Error");
            }
        }
        public async Task<IActionResult> OnPostChangePassword()
        {
            try
            {
                var user = await _userManager.FindByIdAsync(Profile.Id);
                var checkOldPass = await _userManager.CheckPasswordAsync(user, Request.OldPassword);
                if (checkOldPass)
                {
                    if (Request.NewPassword == Request.ConfirmPassword)
                    {
                        var result = await _userManager.ChangePasswordAsync(user, Request.OldPassword, Request.NewPassword);
                        if (result.Succeeded)
                        {
                            TempData["SuccessMessage"] = "Change password successfully";
                            return Page();
                        }
                        TempData["ErrorMessage"] = result.Errors.FirstOrDefault().Description;
                        return Page();
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "New password and confirm password are not the same";
                        return Page();
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Old password is not correct";
                    return Page();
                }


            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToPage("Error");
            }
        }

    }
}
*/