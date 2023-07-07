using System.ComponentModel.DataAnnotations;

namespace StudentManagingSystem_Client.ViewModel
{
    public class UserProfileRequest
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string? Adress { get; set; }
        public string? Email { get; set; }
        public string? Gender { get; set; }
        public DateTime? DOB { get; set; }
        public string? Phone { get; set; }
    }

    public class UserProfileResponse
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string StudentCode { get; set; }
        public string? Adress { get; set; }
        public string? Email { get; set; }
        public string? Gender { get; set; }
        public DateTime? DOB { get; set; }
        public string? Phone { get; set; }
    }
    public class ChangePasswordRequest
    {
        [Required]
        public string OldPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
    }
}
