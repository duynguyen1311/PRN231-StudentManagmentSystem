using DataAccess.Repository;
using DataAccess.Repository.IRepository;
using StudentManagingSystem_API.Configuration.Mappers;

namespace StudentManagingSystem_API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            // Đăng kí automapper
            services.AddAutoMapper(typeof(AutoMapperProfile));

            // Đăng kí mediatR
            //services.AddMediatR(Assembly.GetExecutingAssembly());
            //services.AddMediatR(typeof(ViewUserProfileQuery).Assembly);

            // Đăng kí repository
            services.AddScoped(typeof(IDepartmentRepository), typeof(DepartmentRepository));
            services.AddScoped(typeof(IStudentRepository), typeof(StudentRepository));
            services.AddScoped(typeof(IRoomRepository), typeof(RoomRepository));
            services.AddScoped(typeof(ISubjectRepository), typeof(SubjectRepository));
            services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
            services.AddScoped(typeof(IPointRepository), typeof(PointRepository));
            services.AddScoped(typeof(INotiRepository), typeof(NotiRepository));

            //Đăng kí service
            return services;
        }
    }
}
