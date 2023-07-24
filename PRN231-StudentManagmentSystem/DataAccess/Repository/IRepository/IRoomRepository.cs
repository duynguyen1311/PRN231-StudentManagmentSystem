using BusinessObject.Model;
using BusinessObject.Utility;

namespace DataAccess.Repository.IRepository
{
    public interface IRoomRepository
    {
        Task Add(ClassRoom classRoom, CancellationToken cancellationToken = default);
        Task Update(ClassRoom classRoom, CancellationToken cancellationToken = default);
        Task Delete(Guid id, CancellationToken cancellationToken = default);
        Task DeleteList(List<string> id, CancellationToken cancellationToken = default);
        Task<ClassRoom> GetById(Guid id);
        Task<Guid> GetIdByCode (string? code);
        Task<List<ClassRoom>> GetAll();
        Task<PagedList<ClassRoom>> Search(string? keyword, bool? status,string? tid, int page, int pagesize);
        Task<PagedList<ClassRoom>> SearchClassByStudent(string? keyword, bool? status,Guid? sid, int page, int pagesize);
        Task<List<Student>> ListStudentByClass(Guid sid);
        Task Import(List<ClassRoom> listDept, CancellationToken cancellationToken = default);
        Task<bool> CheckAddExistCode(string code, CancellationToken cancellationToken = default);
    }
}
