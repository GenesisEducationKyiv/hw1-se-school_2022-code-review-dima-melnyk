using GSES.DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GSES.DataAccess.Repositories.Interfaces
{
    public interface ISubscriberRepository
    {
        Task<IEnumerable<Subscriber>> GetAsync();

        Task AddAsync(Subscriber subscriber);
    }
}
