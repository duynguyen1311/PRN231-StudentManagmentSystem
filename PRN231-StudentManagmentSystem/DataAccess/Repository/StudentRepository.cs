using AutoMapper;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Model;
using BusinessObject.Model.Interface;
using DataAccess.Repository.IRepository;
using BusinessObject.Utility;

namespace DataAccess.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ISmsDbContext _context;
        private readonly IMapper _mapper;

        public StudentRepository(ISmsDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task Add(Student student, CancellationToken cancellationToken = default)
        {
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> CheckAddExistEmail(string email, CancellationToken cancellationToken = default)
        {
            List<Student> listS = await _context.Students.ToListAsync();
            foreach (var cus in listS)
            {
                if (email == cus.Email)
                {
                    return false;
                }
            }
            return true;
        }

        public async Task Delete(Guid id, CancellationToken cancellationToken = default)
        {
            var student = await _context.Students.FirstOrDefaultAsync(i => i.Id == id);
            if (student == null) throw new ArgumentException("Can not find !!!");
            _context.Students.Remove(student);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<PagedList<Student>> GetAll(string? keyword, bool? status, int page, int pagesize)
        {
            var query = _context.Students.AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(c => (!string.IsNullOrEmpty(c.StudentName) && c.StudentName.Contains(keyword.ToLower().Trim()))
                                      || (!string.IsNullOrEmpty(c.StudentCode) && c.StudentCode.Contains(keyword.ToLower().Trim())));
            }
            if (status != null)
            {
                query = query.Where(c => c.Status == status);
            }
            var query1 = query.Include(i => i.ClassRoom).ThenInclude(i => i.Department).OrderByDescending(c => c.CreatedDate);
            var query2 = await query1.Skip((page - 1) * pagesize)
                .Take(pagesize).ToListAsync();
            var res = await query.ToListAsync();
            return new PagedList<Student>
            {
                Data = query2,
                TotalCount = res.Count
            };
        }

        public async Task<List<Student>> GetAllWithoutFilter()
        {
            return await _context.Students.Where(i => i.Status == true).OrderByDescending(i => i.CreatedDate).ToListAsync();
        }

        public async Task<Student> GetById(Guid id)
        {
            var student = await _context.Students.FirstOrDefaultAsync(i => i.Id == id);
            if (student == null) throw new ArgumentException("Can not find !!!");
            return student;
        }

        public async Task Update(Student student, CancellationToken cancellationToken = default)
        {
            var oldStudent = await _context.Students.FirstOrDefaultAsync(i => i.Id == student.Id);
            if (oldStudent != null)
            {
                var newStudent = _mapper.Map<Student, Student>(student, oldStudent);
                _context.Students.Update(newStudent);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
