﻿using BusinessObject.Model;
using BusinessObject.Utility;

namespace DataAccess.Repository.IRepository
{
    public interface IDepartmentRepository
    {
        Task Add(Department department, CancellationToken cancellationToken = default);
        Task Update(Department department, CancellationToken cancellationToken = default);
        Task Delete(Guid id, CancellationToken cancellationToken = default);
        Task<Department> GetById(Guid id);
        Task<List<Department>> GetAll();
        Task<PagedList<Department>> Search(string? keyword, bool? status, int page, int pagesize);
    }
}
