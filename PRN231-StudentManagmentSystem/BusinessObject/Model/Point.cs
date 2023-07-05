using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject.Model
{
    public class Point : BaseEntity<Guid>
    {
        [ForeignKey("Student")]
        public Guid StudentId { get; set; }
        public Student? Student { get; set; }
        [ForeignKey("Subject")]
        public Guid SubjectId { get; set; }
        public Subject? Subject { get; set; }
        public float? ProgessPoint { get; set; }
        public float? MidtermPoint { get; set; }
        public bool? IsPassed { get; set; }
        public float? FinalPoint { get; set; }
    }
}
