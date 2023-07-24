using BusinessObject.Model;
using BusinessObject.Utility;

namespace DataAccess.Repository.IRepository
{
    public interface IUserRepository
    {
        Task<List<AppUser>> GetAll();
        Task<AppUser> GetById(string id);
        Task<string> GetIdByEmail(string? email);
        Task Delete(string id, CancellationToken cancellationToken = default);
        Task DeleteList(List<string> id, CancellationToken cancellationToken = default);
        Task<PagedList<AppUser>> Search(string? keyword, bool? status, int page, int pagesize);
        Task<bool> CheckAddExistEmail(string email, CancellationToken cancellationToken = default);
        Task Import(List<AppUser> listDept, CancellationToken cancellationToken = default);
    }
}
