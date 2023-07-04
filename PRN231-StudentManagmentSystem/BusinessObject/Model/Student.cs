using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject.Model
{
    public class Student : BaseEntity<Guid>
    {
        [Required]
        [MaxLength(100)]
        public string StudentName { get; set; }
        [Required]
        [MaxLength(50)]
        public string StudentCode { get; set; }
        [MaxLength(200)]
        public string? Address { get; set; }
        [MaxLength(50)]
        public string? Email { get; set; }
        [MaxLength(10)]
        public string? Gender { get; set; }
        public DateTime? DOB { get; set; }
        public bool? Status { get; set; }
        [MaxLength(50)]
        public string? Phone { get; set; }
        [Required]
        public int InSemester { get; set; }
        [ForeignKey("Class")]
        public Guid? ClassRoomId { get; set; }
        public ClassRoom? ClassRoom { get; set; }
        public ICollection<Point> Point { get; set; }
    }
}
