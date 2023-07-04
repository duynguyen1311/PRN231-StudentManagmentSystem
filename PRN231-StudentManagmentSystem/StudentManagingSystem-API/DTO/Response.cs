using System.Text.Json.Serialization;

namespace StudentManagingSystem_API.DTO
{
    public class ClassRoomSearchResponse
    {
        public Guid Id { get; set; }
        public string? ClassName { get; set; }
        public string? ClassCode { get; set; }
        public bool? Status { get; set; }
        public Guid? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public string? UserId { get; set; }
        public string? TeacherName { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? LastModifiedBy { get; set; }
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
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }

}
