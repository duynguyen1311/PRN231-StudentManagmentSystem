using System.ComponentModel.DataAnnotations;

namespace BusinessObject.Model
{
    public class Department : BaseEntity<Guid>
    {
        [Required]
        [MaxLength(100)]
        public string DepartmentName { get; set; }
        [Required]
        [MaxLength(50)]
        public string DepartmentCode { get; set; }
        public bool? Status { get; set; }
    }
}
