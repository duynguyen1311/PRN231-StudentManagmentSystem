using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace StudentManagingSystem_Client.ViewModel
{
    public class StudentSearchRequest
    {
        public string? keyword { get; set; }
        public bool? status { get; set; }
        public int page { get; set; }
        public int pagesize { get; set; }
    }

    public class StudentAddRequest
    {
        public string StudentName { get; set; }
        public string StudentCode { get; set; }
        public string? ClassCode { get; set; }
        public string? Address { get; set; }
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string? Gender { get; set; }
        public DateTime? DOB { get; set; }
        public bool Status { get; set; }
        public string? Phone { get; set; }
        public int InSemester { get; set; }
        public Guid? ClassRoomId { get; set; }
        [JsonIgnore]
        public string? CreatedBy { get; set; }
        [JsonIgnore]
        public DateTime? CreatedDate { get; set; }
    }
    public class StudentUpdateRequest
    {
        public Guid Id { get; set; }
        public string? StudentName { get; set; }
        public string? StudentCode { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? Gender { get; set; }
        public DateTime? DOB { get; set; }
        public bool? Status { get; set; }
        public string? Phone { get; set; }
        public int InSemester { get; set; }
        public Guid? ClassRoomId { get; set; }
        [JsonIgnore]
        public string? LastModifiedBy { get; set; }
        [JsonIgnore]
        public DateTime? LastModifiedDate { get; set; }
    }
}
