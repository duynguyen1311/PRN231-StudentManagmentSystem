using BusinessObject.Model;
using BusinessObject.Utility;

namespace DataAccess.Repository.IRepository
{
    public interface IUserRepository
    {
        Task<List<AppUser>> GetAll();
        Task<AppUser> GetById(string id);
        Task<PagedList<AppUser>> Search(string? keyword, bool? status, int page, int pagesize);
        Task<bool> CheckAddExistEmail(string email, CancellationToken cancellationToken = default);
    }
}
