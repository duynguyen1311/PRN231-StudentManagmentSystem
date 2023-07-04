using BusinessObject.Model;
using BusinessObject.Utility;

namespace DataAccess.Repository.IRepository
{
    public interface INotiRepository
    {
        Task Add(Notification noti, CancellationToken cancellationToken = default);
        Task Update(Notification noti, CancellationToken cancellationToken = default);
        Task Delete(Guid id, CancellationToken cancellationToken = default);
        Task<Notification> GetById(Guid id);
        Task<List<Notification>> GetAll();
        Task<PagedList<Notification>> Search(int page, int pagesize);
    }
}
