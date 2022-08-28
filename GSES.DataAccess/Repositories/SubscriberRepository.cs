using GSES.DataAccess.Entities;
using GSES.DataAccess.Repositories.Interfaces;
using GSES.DataAccess.Storages.Bases;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GSES.DataAccess.Repositories
{
    public class SubscriberRepository : ISubscriberRepository
    {
        private readonly IStorage storage;

        public SubscriberRepository(IStorage storage)
        {
            this.storage = storage;
        }

        public Task AddAsync(Subscriber subscriber) => this.storage.Subscribers.AddAsync(subscriber);

        public Task<IEnumerable<Subscriber>> GetAsync() => this.storage.Subscribers.GetAllAsync();
    }
}
