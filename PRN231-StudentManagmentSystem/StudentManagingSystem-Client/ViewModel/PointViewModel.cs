using System.Text.Json.Serialization;

namespace StudentManagingSystem_Client.ViewModel
{
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

    public class PointImportRequest
    {
        public string? SubjectCode { get; set; }
        public string? StudentCode { get; set; }
        public float? ProgessPoint { get; set; }
        public float? MidtermPoint { get; set; }

    }

}
