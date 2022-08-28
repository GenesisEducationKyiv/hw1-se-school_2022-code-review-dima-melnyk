using GSES.DataAccess.Entities;

namespace GSES.DataAccess.Storages.Bases
{
    public interface IStorage
    {
        public ITable<Subscriber> Subscribers { get; set; }
    }
}
