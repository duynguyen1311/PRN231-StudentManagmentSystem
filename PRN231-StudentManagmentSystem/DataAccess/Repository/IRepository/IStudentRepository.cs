using BusinessObject.Model;
using BusinessObject.Utility;

namespace DataAccess.Repository.IRepository
{
    public interface IStudentRepository
    {
        Task Add(Student student, CancellationToken cancellationToken = default);
        Task Update(Student student, CancellationToken cancellationToken = default);
        Task Delete(Guid id, CancellationToken cancellationToken = default);
        Task<bool> CheckAddExistEmail(string email, CancellationToken cancellationToken = default);
        Task<Student> GetById(Guid id);
        Task<PagedList<Student>> GetAll(string? keyword, bool? status, int page, int pagesize);
        Task<List<Student>> GetAllWithoutFilter();
        Task Import(List<Student> listDept, CancellationToken cancellationToken = default);
    }
}
