namespace StudentManagingSystem_Client.ViewModel
{
    public class LoginResponseModel
    {
        public string Id { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;

        public string Token { get; set; } = null!;
    }
}
