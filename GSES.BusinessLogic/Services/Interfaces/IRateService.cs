using System.Threading.Tasks;

namespace GSES.BusinessLogic.Services.Interfaces
{
    public interface IRateService
    {
        Task<double> GetRateAsync();
    }
}
