using System.Text.Json.Serialization;

namespace StudentManagingSystem_Client.ViewModel
{
    public class DepartmentSearchRequest
    {
        public string? keyword { get; set; }
        public bool? status { get; set; }
        public int page { get; set; }
        public int pagesize { get; set; }
    }

    public class DepartmentAddRequest
    {
        public string DepartmentName { get; set; }
        public string DepartmentCode { get; set; }
        public bool? Status { get; set; }
        [JsonIgnore]
        public string? CreatedBy { get; set; }
        [JsonIgnore]
        public DateTime? CreatedDate { get; set; }
    }
    public class DepartmentUpdateRequest
    {
        public Guid Id { get; set; }
        public string? DepartmentName { get; set; }
        public string? DepartmentCode { get; set; }
        public bool? Status { get; set; }
        [JsonIgnore]
        public string? LastModifiedBy { get; set; }
        [JsonIgnore]
        public DateTime? LastModifiedDate { get; set; }
    }
    public class DepartmentDeleteListRequest
    {
        public List<string> Id { get; set; }
    }
}
