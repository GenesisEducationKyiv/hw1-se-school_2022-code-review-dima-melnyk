using GSES.BusinessLogic.Processors.Interfaces;
using GSES.BusinessLogic.Services.Interfaces;
using System.Threading.Tasks;

namespace GSES.BusinessLogic.Services
{
    public class RateService : IRateService
    {
        private readonly IRateProcessor rateProcessor;

        public RateService(IRateProcessor rateProcessor)
        {
            this.rateProcessor = rateProcessor;
        }

        public async Task<double> GetRateAsync()
        {
            var model = await this.rateProcessor.GetRateAsync();

            return model.Rate;
        }
    }
}
