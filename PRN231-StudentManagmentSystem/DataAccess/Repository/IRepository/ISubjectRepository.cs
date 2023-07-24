using BusinessObject.Model;
using BusinessObject.Utility;

namespace DataAccess.Repository.IRepository
{
    public interface ISubjectRepository
    {
        Task Add(Subject subject, CancellationToken cancellationToken = default);
        Task Update(Subject subject, CancellationToken cancellationToken = default);
        Task Delete(Guid id, CancellationToken cancellationToken = default);
        Task DeleteList(List<string> id, CancellationToken cancellationToken = default);
        Task<Subject> GetById(Guid id);
        Task<Guid> GetIdByCode(string code);
        Task<List<Subject>> GetAll();
        Task<PagedList<Subject>> Search(string? keyword, bool? status,int? semester, int page, int pagesize);
        Task<PagedList<Subject>> SearchByStudent(string? keyword, bool? status,Guid? stuId, int? semester, int page, int pagesize);
        Task Import(List<Subject> listDept, CancellationToken cancellationToken = default);
        Task<bool> CheckAddExistCode(string code, CancellationToken cancellationToken = default);
    }
}
