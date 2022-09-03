using GSES.DataAccess.Entities;
using GSES.DataAccess.Storages.Bases;
using Microsoft.Extensions.Configuration;

namespace GSES.DataAccess.Storages.File
{
    public class FileStorage : IStorage
    {
        public FileStorage(IConfiguration configuration)
        {
            this.subscribers = new File<Subscriber>(configuration);
        }

        private File<Subscriber> subscribers;

        public ITable<Subscriber> Subscribers
        {
            get => this.subscribers;
            set => this.subscribers = (File<Subscriber>)value;
        }
    }
}
