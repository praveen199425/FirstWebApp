using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using skinet.Core.Specifications;

namespace Core.Interfaces
{
    public interface IGenericRepository<T> where T:BaseEntity
    {
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> GetAllListAsync();
        Task<T> GetEntityWithSpec(ISpecification<T> spec);
        Task<IReadOnlyList<T>> GetListAsync(ISpecification<T> spec);

        Task<int> CountAsync(ISpecification<T> spec);
    }


}