using AutoMapper;
using BusinessObject.Model;
using BusinessObject.Utility;
using StudentManagingSystem_API.DTO;
using System.Threading.Channels;

namespace StudentManagingSystem_API.Configuration.Mappers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {

            #region Request
            //Student
            CreateMap<Student, Student>().ReverseMap().ForAllMembers(x => x.Condition((source, target, sourceValue) => sourceValue != null));
            CreateMap<Student, StudentAddRequest>().ReverseMap().ForAllMembers(x => x.Condition((source, target, sourceValue) => sourceValue != null));
            CreateMap<Student, StudentUpdateRequest>().ReverseMap().ForAllMembers(x => x.Condition((source, target, sourceValue) => sourceValue != null));
            //Department
            CreateMap<Department, Department>().ReverseMap().ForAllMembers(x => x.Condition((source, target, sourceValue) => sourceValue != null));
            CreateMap<Department, DepartmentAddRequest>().ReverseMap().ForAllMembers(x => x.Condition((source, target, sourceValue) => sourceValue != null));
            CreateMap<Department, DepartmentUpdateRequest>().ReverseMap().ForAllMembers(x => x.Condition((source, target, sourceValue) => sourceValue != null));
            //ClassRoom
            CreateMap<ClassRoom, ClassRoom>().ReverseMap().ForAllMembers(x => x.Condition((source, target, sourceValue) => sourceValue != null));
            CreateMap<ClassRoom, ClassRoomAddRequest>().ReverseMap().ForAllMembers(x => x.Condition((source, target, sourceValue) => sourceValue != null));
            CreateMap<ClassRoom, ClassRoomUpdateRequest>().ReverseMap().ForAllMembers(x => x.Condition((source, target, sourceValue) => sourceValue != null));
            CreateMap<ClassRoom, ClassRoomImportRequest>().ReverseMap().ForAllMembers(x => x.Condition((source, target, sourceValue) => sourceValue != null));
            //Subject
            CreateMap<Subject, Subject>().ReverseMap().ForAllMembers(x => x.Condition((source, target, sourceValue) => sourceValue != null));
            CreateMap<Subject, SubjectAddRequest>().ReverseMap().ForAllMembers(x => x.Condition((source, target, sourceValue) => sourceValue != null));
            CreateMap<Subject, SubjectUpdateRequest>().ReverseMap().ForAllMembers(x => x.Condition((source, target, sourceValue) => sourceValue != null));
            //Techer
            CreateMap<AppUser, AppUser>().ReverseMap().ForAllMembers(x => x.Condition((source, target, sourceValue) => sourceValue != null));
            CreateMap<AppUser, TeacherAddRequest>().ReverseMap().ForAllMembers(x => x.Condition((source, target, sourceValue) => sourceValue != null));
            CreateMap<AppUser, TeacherUpdateRequest>().ReverseMap().ForAllMembers(x => x.Condition((source, target, sourceValue) => sourceValue != null));
            //Point
            CreateMap<Point, Point>().ReverseMap().ForAllMembers(x => x.Condition((source, target, sourceValue) => sourceValue != null));
            CreateMap<Point, PointAddRequest>().ReverseMap().ForAllMembers(x => x.Condition((source, target, sourceValue) => sourceValue != null));
            CreateMap<Point, PointUpdateRequest>().ReverseMap().ForAllMembers(x => x.Condition((source, target, sourceValue) => sourceValue != null));
            //Notification
            CreateMap<Notification, Notification>().ReverseMap().ForAllMembers(x => x.Condition((source, target, sourceValue) => sourceValue != null));
            CreateMap<Notification, NotifyAddRequest>().ReverseMap().ForAllMembers(x => x.Condition((source, target, sourceValue) => sourceValue != null));
            CreateMap<Notification, NotifyUpdateRequest>().ReverseMap().ForAllMembers(x => x.Condition((source, target, sourceValue) => sourceValue != null));
            
            //User
            CreateMap<UserProfileRequest, AppUser>().ReverseMap().ForAllMembers(x => x.Condition((source, target, sourceValue) => sourceValue != null));
            
            #endregion

            #region Response
            //ClassRoom
            CreateMap(typeof(PagedList<ClassRoom>), typeof(PagedList<ClassRoomSearchResponse>)).ReverseMap().ForAllMembers(x => x.Condition((source, target, sourceValue) => sourceValue != null)); ;
            CreateMap<ClassRoom, ClassRoomSearchResponse>()
                .ForMember(destination => destination.DepartmentName, options => options.MapFrom(source => source.Department.DepartmentName))
                .ForMember(destination => destination.TeacherName, options => options.MapFrom(source => source.User.FullName));

            //Teacher
            CreateMap(typeof(PagedList<AppUser>), typeof(PagedList<TeacherResponse>)).ReverseMap().ForAllMembers(x => x.Condition((source, target, sourceValue) => sourceValue != null)); ;
            CreateMap<AppUser, TeacherResponse>().ReverseMap().ForAllMembers(x => x.Condition((source, target, sourceValue) => sourceValue != null));
            CreateMap<AppUser, UserProfileResponse>().ReverseMap().ForAllMembers(x => x.Condition((source, target, sourceValue) => sourceValue != null));
            CreateMap<AppUser, TeacherResponse>()
                .ForMember(destination => destination.Status, options => options.MapFrom(source => source.Activated));
            //Point
            CreateMap(typeof(PagedList<Point>), typeof(PagedList<PointResponse>)).ReverseMap().ForAllMembers(x => x.Condition((source, target, sourceValue) => sourceValue != null)); ;
            CreateMap<Point, PointResponse>()
                .ForMember(destination => destination.StudentName, options => options.MapFrom(source => source.Student.StudentName))
                .ForMember(destination => destination.StudentCode, options => options.MapFrom(source => source.Student.StudentCode))
                .ForMember(destination => destination.SubjectName, options => options.MapFrom(source => source.Subject.SubjectName))
                .ForMember(destination => destination.SubjectCode, options => options.MapFrom(source => source.Subject.SubjectCode))
                .ForMember(destination => destination.Semester, options => options.MapFrom(source => source.Subject.Semester));

            #endregion

        }
    }
}
