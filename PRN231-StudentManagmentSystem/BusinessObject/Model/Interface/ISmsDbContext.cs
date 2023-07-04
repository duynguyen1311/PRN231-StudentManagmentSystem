using Microsoft.EntityFrameworkCore;
using System.Threading.Channels;

namespace BusinessObject.Model.Interface
{
    public interface ISmsDbContext
    {

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<ClassRoom> ClassRooms { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Point> Points { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }

    }
}
