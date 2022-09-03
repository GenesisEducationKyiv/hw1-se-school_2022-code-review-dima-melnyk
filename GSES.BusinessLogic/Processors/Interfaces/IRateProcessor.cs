using GSES.BusinessLogic.Models.Rate;
using System.Threading.Tasks;

namespace GSES.BusinessLogic.Processors.Interfaces
{
    public interface IRateProcessor
    {
        Task<BaseRateModel> GetRateAsync();
    }
}
