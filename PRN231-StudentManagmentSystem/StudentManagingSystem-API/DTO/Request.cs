using BusinessObject.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagingSystem_API.DTO
{
    #region Department
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
    #endregion

    #region Notify
    public class NotifySearchRequest
    {
        public int page { get; set; }
        public int pagesize { get; set; }
    }

    public class NotifyAddRequest
    {
        public string Title { get; set; }
        public string? SubTitle { get; set; }
        public string Content { get; set; }
        public bool? Status { get; set; }
        public string? Link { get; set; }
        public bool? IsRead { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
    public class NotifyUpdateRequest
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string? SubTitle { get; set; }
        public string Content { get; set; }
        public bool? Status { get; set; }
        public string? Link { get; set; }
        public bool? IsRead { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }

    #endregion

    #region ClassRoom
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
        public string? UserId { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
    public class ClassRoomUpdateRequest
    {
        public Guid Id { get; set; }
        public string ClassName { get; set; }
        public string ClassCode { get; set; }
        public bool? Status { get; set; }
        public Guid? DepartmentId { get; set; }
        public string? UserId { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
    #endregion
}
