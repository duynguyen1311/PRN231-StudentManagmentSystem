using AutoMapper;
using BusinessObject.Model.Interface;
using BusinessObject.Model;
using DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Utility;

namespace DataAccess.Repository
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly ISmsDbContext _context;
        private readonly IMapper _mapper;

        public SubjectRepository(ISmsDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task Add(Subject subject, CancellationToken cancellationToken = default)
        {
            await _context.Subjects.AddAsync(subject);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> CheckAddExistCode(string code, CancellationToken cancellationToken = default)
        {
            List<Subject> listS = await _context.Subjects.ToListAsync();
            foreach (var cus in listS)
            {
                if (code.Trim() == cus.SubjectCode.Trim())
                {
                    return false;
                }
            }
            return true;
        }

        public async Task Delete(Guid id, CancellationToken cancellationToken = default)
        {
            var subject = await _context.Subjects.FirstOrDefaultAsync(i => i.Id == id);
            if (subject == null) throw new ArgumentException("Can not find !!!");
            _context.Subjects.Remove(subject);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteList(List<string> id, CancellationToken cancellationToken = default)
        {
            var subject = await _context.Subjects.Where(i => id.Contains(i.Id.ToString())).ToListAsync();
            _context.Subjects.RemoveRange(subject);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<Subject>> GetAll()
        {
            var list = await _context.Subjects.Where(c => c.Status == true).OrderByDescending(c => c.CreatedDate).ToListAsync();
            return list;
        }

        public async Task<Subject> GetById(Guid id)
        {
            var subject = await _context.Subjects.FirstOrDefaultAsync(i => i.Id == id);
            if (subject == null) throw new ArgumentException("Can not find !!!");
            return subject;
        }

        public async Task<Guid> GetIdByCode(string code)
        {
            var subject = await _context.Subjects.FirstOrDefaultAsync(i => i.SubjectCode == code);
            if (subject == null) throw new ArgumentException("Can not find !!!");
            return subject.Id;
        }

        public async Task Import(List<Subject> listDept, CancellationToken cancellationToken = default)
        {
            foreach (var item in listDept)
            {
                var dept = await _context.Subjects.FirstOrDefaultAsync(i => i.SubjectCode == item.SubjectCode);
                if (dept == null)
                {
                    var newDept = new Subject()
                    {
                        Id = Guid.NewGuid(),
                        SubjectCode = item.SubjectCode,
                        SubjectName = item.SubjectName,
                        Semester = item.Semester,
                        Credit = item.Credit,
                        Description = item.Description,
                        Status = true,
                        CreatedDate = DateTime.Now,
                    };
                    await _context.Subjects.AddAsync(newDept);
                    await _context.SaveChangesAsync(cancellationToken);
                }

            }
        }

        public async Task<PagedList<Subject>> Search(string? keyword, bool? status, int? semester, int page, int pagesize)
        {
            var query = _context.Subjects.AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(c => (!string.IsNullOrEmpty(c.SubjectName) && c.SubjectName.Contains(keyword.ToLower().Trim()))
                                      || (!string.IsNullOrEmpty(c.SubjectCode) && c.SubjectCode.Contains(keyword.ToLower().Trim())));
            }
            if (status != null)
            {
                query = query.Where(c => c.Status == status);
            }
            if (semester != null)
            {
                query = query.Where(i => i.Semester == semester);
            }
            var query1 = query.OrderByDescending(c => c.CreatedDate);
            var query2 = await query1.Skip((page - 1) * pagesize)
                .Take(pagesize).ToListAsync();
            var res = await query.ToListAsync();
            return new PagedList<Subject>
            {
                Data = query2,
                TotalCount = res.Count
            };
        }

        public async Task<PagedList<Subject>> SearchByStudent(string? keyword, bool? status, Guid? stuId, int? semester, int page, int pagesize)
        {
            var query = _context.Points.AsQueryable();
            var listSub = new List<Subject>();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(c => (!string.IsNullOrEmpty(c.Subject.SubjectName) && c.Subject.SubjectName.Contains(keyword.ToLower().Trim()))
                                      || (!string.IsNullOrEmpty(c.Subject.SubjectCode) && c.Subject.SubjectCode.Contains(keyword.ToLower().Trim())));
            }
            if (status != null)
            {
                query = query.Where(c => c.Subject.Status == status);
            }
            if (stuId != null)
            {
                if (stuId == Guid.Empty)
                {
                    query = query.Where(i => i.Student.Id == null);
                }
                query = query.Where(i => i.Student.Id == stuId);
            }
            if (semester != null)
            {
                query = query.Where(i => i.Subject.Semester == semester);
            }
            var query1 = query.Include(i => i.Subject).OrderByDescending(c => c.CreatedDate);
            var query2 = await query1.Skip((page - 1) * pagesize)
                .Take(pagesize).ToListAsync();
            var res = await query1.ToListAsync();
            listSub = query2.Select(i => i.Subject).ToList();
            return new PagedList<Subject>
            {
                Data = listSub,
                TotalCount = res.Count
            };
        }

        public async Task Update(Subject subject, CancellationToken cancellationToken = default)
        {
            var dept = await _context.Subjects.FirstOrDefaultAsync(i => i.Id == subject.Id);
            if (dept != null)
            {
                var newDept = _mapper.Map<Subject, Subject>(subject, dept);
                _context.Subjects.Update(newDept);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
