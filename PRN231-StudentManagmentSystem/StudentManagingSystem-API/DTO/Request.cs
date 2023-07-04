using System.ComponentModel.DataAnnotations;

namespace StudentManagingSystem_API.DTO
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
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
    public class DepartmentUpdateRequest
    {
        public Guid Id { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentCode { get; set; }
        public bool? Status { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
