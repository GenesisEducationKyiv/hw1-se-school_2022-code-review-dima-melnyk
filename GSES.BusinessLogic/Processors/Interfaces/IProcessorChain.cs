using GSES.BusinessLogic.Models.Rate;
using System.Threading.Tasks;

namespace GSES.BusinessLogic.Processors.Interfaces
{
    public interface IProcessorChain
    {
        public IProcessorChain Next { get; set; }

        public Task<BaseRateModel> Handle();
    }
}
