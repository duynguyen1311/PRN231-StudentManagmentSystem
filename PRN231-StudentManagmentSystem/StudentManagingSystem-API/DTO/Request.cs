using BusinessObject.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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

    public class ImportDepartmentRequest
    {
        public List<Department> listDepartment { get; set; }
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
        [JsonIgnore]
        public string? CreatedBy { get; set; }
        [JsonIgnore]
        public DateTime? CreatedDate { get; set; }
    }
    public class NotifyUpdateRequest
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? SubTitle { get; set; }
        public string Content { get; set; }
        public bool? Status { get; set; }
        public string? Link { get; set; }
        public bool? IsRead { get; set; }
        [JsonIgnore]
        public string? LastModifiedBy { get; set; }
        [JsonIgnore]
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
    public class ClassRoomImportRequest
    {
        public string ClassName { get; set; }
        public string ClassCode { get; set; }
        public bool? Status { get; set; }
        public string? DepartmentCode { get; set; }
        public string? Email { get; set; }
        public Guid? DepartmentId { get; set; }
        public string? UserId { get; set; }
    }

    public class ClassRoomAddRequest
    {
        public string ClassName { get; set; }
        public string ClassCode { get; set; }
        public bool? Status { get; set; }
        public Guid? DepartmentId { get; set; }
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
    #endregion

    #region Student
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
    #endregion

    #region Subject
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
    #endregion

    #region Point
    public class PointSearchRequest
    {
        public string? keyword { get; set; }
        public int? semester { get; set; }
        public Guid? subjectId { get; set; }
        public Guid? studentId { get; set; }
        public Guid? classId { get; set; }
        public int page { get; set; }
        public int pagesize { get; set; }
    }

    public class PointImportRequest
    {
        public string? SubjectCode { get; set; }
        public string? StudentCode { get; set;}
        public float? ProgessPoint { get; set; }
        public float? MidtermPoint { get; set; }

    }

    public class PointAddRequest
    {
        [JsonIgnore]
        public Guid? Id { get; set; }
        public Guid StudentId { get; set; }
        public Guid SubjectId { get; set; }
        public float? ProgessPoint { get; set; }
        public float? MidtermPoint { get; set; }
        [JsonIgnore]
        public string? CreatedBy { get; set; }
        [JsonIgnore]
        public DateTime? CreatedDate { get; set; }
    }
    public class PointUpdateRequest
    {
        public Guid Id { get; set; }
        public float? ProgessPoint { get; set; }
        public float? MidtermPoint { get; set; }
        [JsonIgnore]
        public string? LastModifiedBy { get; set; }
        [JsonIgnore]
        public DateTime? LastModifiedDate { get; set; }
    }
    #endregion

    #region Teacher
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
        public bool? Activated { get; set; }
        [JsonIgnore]
        public string? LastModifiedBy { get; set; }
        [JsonIgnore]
        public DateTime? LastModifiedDate { get; set; }
    }
    #endregion

    #region Login
    public class LoginRequestModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
    #endregion

    #region User
    public class ForgotPasswordRequest
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }

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

    public class ChangePasswordRequest
    {
        [Required]
        public string OldPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
    }
    #endregion
}
