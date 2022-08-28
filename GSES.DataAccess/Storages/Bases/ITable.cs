using GSES.DataAccess.Entities.Bases;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GSES.DataAccess.Storages.Bases
{
    public interface ITable<T> where T: BaseEntity
    {
        Task AddAsync(T element);
        Task<IEnumerable<T>> GetAsync(Func<T, bool> predicate);
        Task<IEnumerable<T>> GetAllAsync();
        Task UpdateAsync(T element);
        Task DeleteAsync(T element);
    }
}
