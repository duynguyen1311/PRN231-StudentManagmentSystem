using BusinessObject.Model;
using System.ComponentModel.DataAnnotations.Schema;
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

    public class StudentExport
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
        public string? ClassName { get; set; }
    }

    public class PointExport
    {
        public Guid Id { get; set; }
        public string? StudentName { get; set; }
        public string? StudentCode { get; set; }
        public string? SubjectName { get; set; }
        public string? SubjectCode { get; set; }
        public int? Semester { get; set; }
        public float? ProgessPoint { get; set; }
        public float? MidtermPoint { get; set; }
        public float? FinalPoint { get; set; }
        public string? Result { get; set; }
    }

    public class ClassRoomExport
    {
        public Guid Id { get; set; }
        public string? ClassName { get; set; }
        public string? ClassCode { get; set; }
        public bool? Status { get; set; }
        public string? Department { get; set; }
        public string? Teacher { get; set; }
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

    public class PointResponse
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public string? StudentName { get; set; }
        public string? StudentCode { get; set; }
        public Guid SubjectId { get; set; }
        public string? SubjectName { get; set; }
        public string? SubjectCode { get; set; }
        public float? ProgessPoint { get; set; }
        public float? MidtermPoint { get; set; }
        public int? Semester { get; set; }
        public bool? IsPassed { get; set; }
        public float? FinalPoint { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }

    public class UserProfileResponse
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string StudentCode { get; set; }
        public string? Adress { get; set; }
        public string? Email { get; set; }
        public string? Gender { get; set; }
        public DateTime? DOB { get; set; }
        public string? Phone { get; set; }
    }

    public class PointStatic
    {
        
    }

    public class LoginResponse
    {
        public string Id { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;

        public string Token { get; set; } = null!;
    }

}
