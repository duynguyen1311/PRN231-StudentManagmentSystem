using BusinessObject.Model;
using BusinessObject.Utility;

namespace DataAccess.Repository.IRepository
{
    public interface IPointRepository
    {
        Task Add(Point point, CancellationToken cancellationToken = default);
        Task Update(Point point, CancellationToken cancellationToken = default);
        Task Delete(Guid id, CancellationToken cancellationToken = default);
        Task DeleteList(List<string> listId, CancellationToken cancellationToken = default);
        Task<Point> GetById(Guid id);
        Task<PagedList<Point>> Search(string? keyword, int? semester, Guid? subId, Guid? stuId, Guid? cid, int page, int pagesize);
        Task<PagedList<Point>> PointStatistic(string? keyword, int? semester, Guid? subId, Guid? stuId, int page, int pagesize);
        Task Import(List<Point> listPoint, CancellationToken cancellationToken = default);
        Task<bool> CheckExistId(Guid studentId, Guid subjectId);
    }
}
