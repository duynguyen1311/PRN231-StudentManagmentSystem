using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Model.Interface;
using BusinessObject.Model.SeedData;

namespace BusinessObject.Model
{
    public class SmsDbContext : IdentityDbContext, ISmsDbContext
    {
        public SmsDbContext(DbContextOptions<SmsDbContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<ClassRoom> ClassRooms { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Point> Points { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            //new SeedDataApplicationDatabaseContext(builder).Seed();

            // Rename AspNet default tables
            //builder.Entity<AppUser>().ToTable("Users");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");

            builder.Entity<Point>()
                .HasKey(p => new { p.SubjectId, p.StudentId });
            builder.Entity<Point>()
                .HasOne(p => p.Subject)
                .WithMany(p => p.Point)
                .HasForeignKey(p => p.SubjectId);
            builder.Entity<Point>()
                .HasOne(p => p.Student)
                .WithMany(p => p.Point)
                .HasForeignKey(p => p.StudentId);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return (await base.SaveChangesAsync(true, cancellationToken));
        }
    }
}
