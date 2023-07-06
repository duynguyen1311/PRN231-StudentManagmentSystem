using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace StudentManagingSystem_Client.ViewModel
{
    public class TeacherSearchRequest
    {
        public string? keyword { get; set; }
        public bool? status { get; set; }
        public int page { get; set; }
        public int pagesize { get; set; }
    }

    public class TeacherAddRequest
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string? Adress { get; set; }
        public string? Phone { get; set; }
        public string? Gender { get; set; }
        public DateTime? DOB { get; set; }
        [Required]
        public bool Status { get; set; }
        [JsonIgnore]
        public string? CreatedBy { get; set; }
        [JsonIgnore]
        public DateTime? CreatedDate { get; set; }
    }
    public class TeacherUpdateRequest
    {
        public string Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Adress { get; set; }
        public string? Phone { get; set; }
        public string? Gender { get; set; }
        public DateTime? DOB { get; set; }
        [JsonIgnore]
        public string? LastModifiedBy { get; set; }
        [JsonIgnore]
        public DateTime? LastModifiedDate { get; set; }
    }
    public class TeacherResponse
    {
        public string Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Adress { get; set; }
        public string? Phone { get; set; }
        public string? Gender { get; set; }
        public DateTime? DOB { get; set; }
        public bool Status { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
