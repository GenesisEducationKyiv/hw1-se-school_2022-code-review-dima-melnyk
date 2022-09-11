using GSES.BusinessLogic.Models.Rate;
using GSES.BusinessLogic.Processors.Interfaces;
using GSES.BusinessLogic.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace GSES.BusinessLogic.Services
{
    public class RateService : IRateService
    {
        private static object cacheKey = new object();

        private readonly IProcessorChain rateProcessor;
        private readonly IMemoryCache cache;

        public RateService(IProcessorChain rateProcessor, IMemoryCache cache)
        {
            this.rateProcessor = rateProcessor;
            this.cache = cache;
        }

        public async Task<double> GetRateAsync()
        {
            if (!this.cache.TryGetValue(cacheKey, out BaseRateModel cachedModel))
            {
                cachedModel = await this.rateProcessor.Handle();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                this.cache.Set(cacheKey, cachedModel, cacheEntryOptions);
            }

            return cachedModel.Rate;
        }
    }
}
