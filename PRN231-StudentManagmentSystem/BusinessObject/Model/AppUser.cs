using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using BusinessObject.Model.Interface;

namespace BusinessObject.Model
{
    public class AppUser : IdentityUser, IAuditedEntityBase
    {
        public string Login
        {
            get => UserName;
            set => UserName = value;
        }

        [StringLength(50)]
        [Column("FullName")]
        public string FullName { get; set; }

        [StringLength(200)]
        [Column("Address")]
        public string? Adress { get; set; }

        [StringLength(100)]
        [Column("Phone")]
        public string? Phone { get; set; }
        [MaxLength(10)]
        public string? Gender { get; set; }
        public DateTime? DOB { get; set; }

        [Required] public int Type { get; set; }

        [Required] public bool Activated { get; set; }

        

        [Url]
        [StringLength(256)]
        [Column("image_url")]
        public string? ImageUrl { get; set; }

        [StringLength(20)]
        [Column("activation_key")]
        [JsonIgnore]
        public string? ActivationKey { get; set; }

        [StringLength(20)]
        [Column("reset_key")]
        [JsonIgnore]
        public string? ResetKey { get; set; }

        [Column("reset_date")] public DateTime? ResetDate { get; set; }

        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public override bool Equals(object obj)
        {
            if (this == obj) return true;

            if (obj == null || GetType() != obj.GetType()) return false;

            var user = obj as AppUser;
            if (user?.Id == null || Id == null) return false;

            return EqualityComparer<string>.Default.Equals(Id, user.Id);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

        
    }
}
