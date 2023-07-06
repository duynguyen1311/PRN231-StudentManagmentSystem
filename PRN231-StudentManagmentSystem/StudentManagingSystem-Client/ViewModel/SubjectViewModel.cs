using System.Text.Json.Serialization;

namespace StudentManagingSystem_Client.ViewModel
{
    public class SubjectSearchRequest
    {
        public string? keyword { get; set; }
        public bool? status { get; set; }
        public int? semester { get; set; }
        public int page { get; set; }
        public int pagesize { get; set; }
    }

    public class SubjectSearchByStudentRequest
    {
        public string? keyword { get; set; }
        public bool? status { get; set; }
        public Guid? studentId { get; set; }
        public int? semester { get; set; }
        public int page { get; set; }
        public int pagesize { get; set; }
    }

    public class SubjectAddRequest
    {
        public string SubjectName { get; set; }
        public string SubjectCode { get; set; }
        public bool? Status { get; set; }
        public string? Description { get; set; }
        public int? Semester { get; set; }
        //tín chỉ
        public int? Credit { get; set; }
        [JsonIgnore]
        public string? CreatedBy { get; set; }
        [JsonIgnore]
        public DateTime? CreatedDate { get; set; }
    }
    public class SubjectUpdateRequest
    {
        public Guid Id { get; set; }
        public string? SubjectName { get; set; }
        public string? SubjectCode { get; set; }
        public bool? Status { get; set; }
        public string? Description { get; set; }
        public int? Semester { get; set; }
        [JsonIgnore]
        public string? LastModifiedBy { get; set; }
        [JsonIgnore]
        public DateTime? LastModifiedDate { get; set; }
    }
}
