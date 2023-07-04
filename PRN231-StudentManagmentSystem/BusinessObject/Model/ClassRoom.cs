using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject.Model
{
    public class ClassRoom : BaseEntity<Guid>
    {
        [Required]
        [MaxLength(100)]
        public string ClassName { get; set; }
        [Required]
        [MaxLength(50)]
        public string ClassCode { get; set; }
        public bool? Status { get; set; }
        [ForeignKey("Department")]
        public Guid? DepartmentId { get; set; }
        public Department? Department { get; set; }
        [ForeignKey("User")]
        public string? UserId { get; set; }
        public AppUser? User { get; set; }
    }
}
