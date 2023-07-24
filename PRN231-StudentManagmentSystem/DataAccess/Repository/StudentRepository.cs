using AutoMapper;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Model;
using BusinessObject.Model.Interface;
using DataAccess.Repository.IRepository;
using BusinessObject.Utility;
using Microsoft.AspNetCore.Identity;

namespace DataAccess.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ISmsDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public StudentRepository(ISmsDbContext context, IMapper mapper, UserManager<AppUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }
        public async Task Add(Student student, CancellationToken cancellationToken = default)
        {
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> CheckAddExistCode(string code, CancellationToken cancellationToken = default)
        {
            List<Student> listS = await _context.Students.ToListAsync();
            foreach (var cus in listS)
            {
                if (code.Trim() == cus.StudentCode.Trim())
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<bool> CheckAddExistEmail(string email, CancellationToken cancellationToken = default)
        {
            List<Student> listS = await _context.Students.ToListAsync();
            foreach (var cus in listS)
            {
                if (email.Trim() == cus.Email.Trim())
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
            student.Status = false;
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteList(List<string> id, CancellationToken cancellationToken = default)
        {
            var student = await _context.Students.Where(i => id.Contains(i.Id.ToString())).ToListAsync();
            foreach (var item in student)
            {
                item.Status = false;
            }
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

        public async Task<Guid> GetIdByCode(string code)
        {
            var student = await _context.Students.FirstOrDefaultAsync(i => i.StudentCode == code);
            if (student == null) throw new ArgumentException("Can not find !!!");
            return student.Id;
        }

        public async Task<List<Student>> GetStudentByClass(Guid? classId)
        {
            return await _context.Students.Where(i => i.Status == true && i.ClassRoomId == classId).OrderByDescending(i => i.CreatedDate).ToListAsync();
        }

        public async Task Import(List<Student> listDept, CancellationToken cancellationToken = default)
        {
            foreach (var item in listDept)
            {
                var dept = await _context.Students.FirstOrDefaultAsync(i => i.StudentCode == item.StudentCode);
                if (dept == null)
                {
                    var newDept = new Student()
                    {
                        Id = Guid.NewGuid(),
                        StudentCode = item.StudentCode,
                        StudentName = item.StudentName,
                        Email = item.Email,
                        Address = item.Address,
                        Phone = item.Phone,
                        DOB = item.DOB,
                        Gender = item.Gender,
                        InSemester = item.InSemester,
                        ClassRoomId = item.ClassRoomId,
                        Status = true,
                        CreatedDate = DateTime.Now,
                    };
                    await _context.Students.AddAsync(newDept);
                    await _context.SaveChangesAsync(cancellationToken);

                    var user = new AppUser()
                    {
                        Id = newDept.Id.ToString(),
                        FullName = newDept.StudentName,
                        Login = newDept.Email,
                        Email = newDept.Email,
                        Adress = newDept.Address,
                        PhoneNumber = newDept.Phone,
                        DOB = item.DOB,
                        Gender = item.Gender,
                        UserName = newDept.Email,
                        Activated = true,
                        Type = 0,
                        CreatedDate = DateTime.Now,
                    };
                    var result = await _userManager.CreateAsync(user, "Abc@123");
                    if (result.Succeeded) await _userManager.AddToRoleAsync(user, RoleConstant.STUDENT);
                }

            }
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
