using System.Text.Json.Serialization;

namespace StudentManagingSystem_Client.ViewModel
{
    public class ClassRoomSearchRequest
    {
        public string? keyword { get; set; }
        public bool? status { get; set; }
        public string? teacherId { get; set; }
        public int page { get; set; }
        public int pagesize { get; set; }
    }

    public class ClassRoomSearchByStudentRequest
    {
        public string? keyword { get; set; }
        public bool? status { get; set; }
        public Guid? studentId { get; set; }
        public int page { get; set; }
        public int pagesize { get; set; }
    }

    public class ClassRoomAddRequest
    {
        public string ClassName { get; set; }
        public string ClassCode { get; set; }
        public bool? Status { get; set; }
        public Guid? DepartmentId { get; set; }
        [JsonPropertyName("teacherId")]
        public string? UserId { get; set; }
        [JsonIgnore]
        public string? CreatedBy { get; set; }
        [JsonIgnore]
        public DateTime? CreatedDate { get; set; }
    }
    public class ClassRoomUpdateRequest
    {
        public Guid Id { get; set; }
        public string? ClassName { get; set; }
        public string? ClassCode { get; set; }
        public bool? Status { get; set; }
        public Guid? DepartmentId { get; set; }
        public string? UserId { get; set; }
        [JsonIgnore]
        public string? LastModifiedBy { get; set; }
        [JsonIgnore]
        public DateTime? LastModifiedDate { get; set; }
    }
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

    public class ClassRoomImportRequest
    {
        public string? ClassName { get; set; }
        public string? ClassCode { get; set; }
        public string? DepartmentCode { get; set; }
        public string? Email { get; set; }
    }
}
