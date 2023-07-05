using System.ComponentModel.DataAnnotations;

namespace BusinessObject.Model
{
    public class Subject : BaseEntity<Guid>
    {
        [Required]
        [MaxLength(100)]
        public string SubjectName { get; set; }
        [Required]
        [MaxLength(50)]
        public string SubjectCode { get; set; }
        public bool? Status { get; set; }
        [MaxLength(500)]
        public string? Description { get; set; }
        //Tín chỉ
        public int? Credit { get; set; }
        public int? Semester { get; set; }
        public ICollection<Point> Point { get; set; }
    }
}
