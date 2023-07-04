using AutoMapper;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Model;
using BusinessObject.Model.Interface;
using DataAccess.Repository.IRepository;
using BusinessObject.Utility;

namespace DataAccess.Repository
{
    public class NotiRepository : INotiRepository
    {
        private readonly ISmsDbContext _context;
        private readonly IMapper _mapper;

        public NotiRepository (ISmsDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task Add(Notification noti, CancellationToken cancellationToken = default)
        {
            await _context.Notifications.AddAsync(noti);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task Delete(Guid id, CancellationToken cancellationToken = default)
        {
            var noti = await _context.Notifications.FirstOrDefaultAsync(i => i.Id == id);
            if(noti == null) throw new ArgumentException("Can not find !!!");
            _context.Notifications.Remove(noti);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<Notification>> GetAll()
        {
            var list = await _context.Notifications.Where(c => c.Status == true).OrderByDescending(c => c.CreatedDate).ToListAsync();
            return list;
        }

        public async Task<Notification> GetById(Guid id)
        {
            var noti = await _context.Notifications.FirstOrDefaultAsync(i => i.Id == id);
            if (noti == null) throw new ArgumentException("Can not find !!!");
            return noti;
        }

        public async Task<PagedList<Notification>> Search(int page, int pagesize)
        {
            var query = _context.Notifications.AsQueryable();
            var res = await query.ToListAsync();
            var list = await query.OrderByDescending(c => c.CreatedDate)
                .Skip((page - 1) * pagesize)
                .Take(pagesize).ToListAsync();
            return new PagedList<Notification>
            {
                Data = list,
                TotalCount = res.Count
            };
        }

        public async Task Update(Notification noti, CancellationToken cancellationToken = default)
        {
            var dept = await _context.Notifications.FirstOrDefaultAsync(i => i.Id == noti.Id);
            if (dept != null)
            {
                var newDept = _mapper.Map<Notification, Notification>(noti, dept);
                _context.Notifications.Update(newDept);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
