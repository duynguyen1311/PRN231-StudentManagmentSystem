using AutoMapper;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Model;
using BusinessObject.Model.Interface;
using BusinessObject.Utility;
using DataAccess.Repository.IRepository;

namespace DataAccess.Repository
{
    public class PointRepository : IPointRepository
    {
        private readonly ISmsDbContext _context;
        private readonly IMapper _mapper;

        public PointRepository(ISmsDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task Add(Point point, CancellationToken cancellationToken = default)
        {
            point.FinalPoint = (point.MidtermPoint * 50) / 100 + (point.ProgessPoint * 50) / 100;
            if (point.FinalPoint >= 5) point.IsPassed = true;
            else point.IsPassed = false;
            await _context.Points.AddAsync(point);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> CheckExistId(Guid studentId, Guid subjectId)
        {
            var point = await _context.Points.ToListAsync();
            foreach(var item in point)
            {
                if (item.StudentId == studentId && item.SubjectId == subjectId)
                {
                    return false;
                }
            }
            return true;
        }

        public async Task Delete(Guid id, CancellationToken cancellationToken = default)
        {
            var point = await _context.Points.FirstOrDefaultAsync(i => i.Id == id);
            if (point == null) throw new ArgumentException("Can not find !!!");
            _context.Points.Remove(point);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteList(List<string> listId, CancellationToken cancellationToken = default)
        {
            var listPoint = await _context.Points.Where(i => listId.Contains(i.Id.ToString())).ToListAsync();
            _context.Points.RemoveRange(listPoint);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<Point> GetById(Guid id)
        {
            var point = await _context.Points.FirstOrDefaultAsync(i => i.Id == id);
            if (point == null) throw new ArgumentException("Can not find !!!");
            return point;
        }

        public async Task Import(List<Point> listPoint, CancellationToken cancellationToken = default)
        {
            foreach (var item in listPoint)
            {
                var dept = await _context.Points.FirstOrDefaultAsync(i => i.StudentId == item.StudentId && i.SubjectId == item.SubjectId);
                if (dept == null)
                {
                    var newDept = new Point()
                    {
                        Id = Guid.NewGuid(),
                        SubjectId = item.SubjectId,
                        StudentId = item.StudentId,
                        ProgessPoint = item.ProgessPoint,
                        MidtermPoint = item.MidtermPoint,
                        FinalPoint = (item.MidtermPoint * 50) / 100 + (item.ProgessPoint * 50) / 100,
                        IsPassed = (item.MidtermPoint * 50) / 100 + (item.ProgessPoint * 50) / 100 >= 5 ? true : false,
                        CreatedDate = DateTime.Now,
                    };
                    await _context.Points.AddAsync(newDept);
                    await _context.SaveChangesAsync(cancellationToken);
                }

            }
        }

        public async Task<PagedList<Point>> PointStatistic(string? keyword, int? semester, Guid? subId, Guid? stuId, int page, int pagesize)
        {
            var query = _context.Points.AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(c => !string.IsNullOrEmpty(c.Subject.SubjectName) && c.Subject.SubjectName.Contains(keyword.ToLower().Trim())
                                      || !string.IsNullOrEmpty(c.Subject.SubjectCode) && c.Subject.SubjectCode.Contains(keyword.ToLower().Trim())
                                      || !string.IsNullOrEmpty(c.Student.StudentName) && c.Student.StudentName.Contains(keyword.ToLower().Trim())
                                      || !string.IsNullOrEmpty(c.Student.StudentCode) && c.Student.StudentCode.Contains(keyword.ToLower().Trim()));
            }
            if (subId != null)
            {
                if (subId == Guid.Empty)
                {
                    query = query.Where(i => i.Subject.Id == null);
                }
                query = query.Where(i => i.Subject.Id == subId);
            }

            if (semester != null)
            {
                query = query.Where(i => i.Subject.Semester == semester);
            }

            if (stuId != null)
            {
                if (stuId == Guid.Empty)
                {
                    query = query.Where(i => i.Subject.Id == null);
                }
                query = query.Where(i => i.Subject.Id == stuId);
            }

            var query1 = query.Include(i => i.Subject).Include(i => i.Student).OrderByDescending(c => c.CreatedDate);
            var query2 = await query1.Skip((page - 1) * pagesize)
                .Take(pagesize).ToListAsync();
            var res = await query1.AsNoTracking().ToListAsync();

            return new PagedList<Point>
            {
                Data = query2,
                TotalCount = res.Count
            };
        }

        public async Task<PagedList<Point>> Search(string? keyword, int? semester, Guid? subId, Guid? stuId,Guid? cid, int page, int pagesize)
        {
            var query = _context.Points.AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(c => !string.IsNullOrEmpty(c.Subject.SubjectName) && c.Subject.SubjectName.Contains(keyword.ToLower().Trim())
                                      || !string.IsNullOrEmpty(c.Subject.SubjectCode) && c.Subject.SubjectCode.Contains(keyword.ToLower().Trim())
                                      || !string.IsNullOrEmpty(c.Student.StudentName) && c.Student.StudentName.Contains(keyword.ToLower().Trim())
                                      || !string.IsNullOrEmpty(c.Student.StudentCode) && c.Student.StudentCode.Contains(keyword.ToLower().Trim()));
            }
            if (subId != null)
            {
                if (subId == Guid.Empty)
                {
                    query = query.Where(i => i.SubjectId == null);
                }
                query = query.Where(i => i.SubjectId == subId);
            }

            if (stuId != null)
            {
                if (stuId == Guid.Empty)
                {
                    query = query.Where(i => i.StudentId == null);
                }
                query = query.Where(i => i.StudentId == stuId);
            }

            if (cid != null)
            {
                if (cid == Guid.Empty)
                {
                    query = query.Where(i => i.Student.ClassRoomId == null);
                }
                query = query.Where(i => i.Student.ClassRoomId == cid);
            }

            if (semester != null)
            {
                query = query.Where(i => i.Subject.Semester == semester);
            }
            var query1 = query.Include(i => i.Subject).Include(i => i.Student).OrderByDescending(c => c.CreatedDate);
            var query2 = await query1.Skip((page - 1) * pagesize)
                .Take(pagesize).ToListAsync();
            var res = await query1.AsNoTracking().ToListAsync();

            return new PagedList<Point>
            {
                Data = query2,
                TotalCount = res.Count
            };
        }

        public async Task Update(Point point, CancellationToken cancellationToken = default)
        {
            var dept = await _context.Points.FirstOrDefaultAsync(i => i.Id == point.Id);
            if (dept == null) throw new ArgumentException("Can not find !");
            point.SubjectId = dept.SubjectId;
            point.StudentId = dept.StudentId;
            _mapper.Map(point, dept);
            dept.FinalPoint = (dept.MidtermPoint * 50) / 100 + (dept.ProgessPoint * 50) / 100;
            if (dept.FinalPoint >= 5) dept.IsPassed = true;
            else dept.IsPassed = false;
            await _context.SaveChangesAsync(cancellationToken);

        }
    }
}
