using System.Threading.Tasks;

namespace GSES.BusinessLogic.Services.Interfaces
{
    public interface ISubscriberService
    {
        Task AddSubscriberAsync(string email);

        Task SendRateForAllTheSubscribers();
    }
}
