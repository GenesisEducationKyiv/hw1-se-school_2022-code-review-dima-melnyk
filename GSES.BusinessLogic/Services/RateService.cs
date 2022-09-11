using GSES.BusinessLogic.Processors.Interfaces;
using GSES.BusinessLogic.Services.Interfaces;
using System.Threading.Tasks;

namespace GSES.BusinessLogic.Services
{
    public class RateService : IRateService
    {
        private readonly IProcessorChain rateProcessor;

        public RateService(IProcessorChain rateProcessor)
        {
            this.rateProcessor = rateProcessor;
        }

        public async Task<double> GetRateAsync()
        {
            var model = await this.rateProcessor.Handle();

            return model.Rate;
        }
    }
}
