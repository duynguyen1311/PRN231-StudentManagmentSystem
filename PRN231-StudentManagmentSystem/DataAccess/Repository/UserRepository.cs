using AutoMapper;
using BusinessObject.Model.Interface;
using BusinessObject.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DataAccess.Repository.IRepository;
using BusinessObject.Utility;

namespace DataAccess.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ISmsDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public UserRepository(ISmsDbContext context, IMapper mapper, UserManager<AppUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<List<AppUser>> GetAll()
        {
            var list = await _context.AppUsers.Where(i => i.Activated && i.Type == 1).OrderByDescending(i => i.CreatedDate).ToListAsync();
            return list;
        }
        public async Task<bool> CheckAddExistEmail(string email, CancellationToken cancellationToken = default)
        {
            List<AppUser> listS = await _context.AppUsers.ToListAsync();
            foreach (var cus in listS)
            {
                if (email == cus.Email)
                {
                    return false;
                }
            }
            return true;
        }
        public async Task<AppUser> GetById(string id)
        {
            var User = await _userManager.FindByIdAsync(id);
            if (User == null) throw new ArgumentException("Can not find !!!");
            return User;
        }

        public async Task<PagedList<AppUser>> Search(string? keyword, bool? status, int page, int pagesize)
        {
            var query = _context.AppUsers.Where(i => i.Type == 1).AsQueryable();
            var res = await query.ToListAsync();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(c => (!string.IsNullOrEmpty(c.FullName) && c.FullName.Contains(keyword.ToLower().Trim()))
                                      || (!string.IsNullOrEmpty(c.Email) && c.Email.Contains(keyword.ToLower().Trim())));
            }
            if (status != null)
            {
                query = query.Where(c => c.Activated == status);
            }
            var list = await query.OrderByDescending(c => c.CreatedDate)
                .Skip((page - 1) * pagesize)
                .Take(pagesize).ToListAsync();
            return new PagedList<AppUser>
            {
                Data = list,
                TotalCount = res.Count
            };
        }
    }
}
