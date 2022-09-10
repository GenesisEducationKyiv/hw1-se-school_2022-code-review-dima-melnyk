using GSES.BusinessLogic.Consts;
using GSES.BusinessLogic.Extensions;
using GSES.BusinessLogic.Models.Rate;
using GSES.BusinessLogic.Processors.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace GSES.BusinessLogic.Processors
{
    public class CoingeckoRateProcessor : IRateProcessor
    {
        private readonly HttpClient httpClient;
        private readonly IConfiguration configuration;

        public CoingeckoRateProcessor(HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;
            this.configuration = configuration;
        }

        public async Task<BaseRateModel> GetRateAsync()
        {
            var url = this.configuration[RateConsts.CoingeckoUrlKey];

            var responseResult = await this.httpClient.GetModelFromRequest<Dictionary<string, Dictionary<string, CoingeckoRateModel>>>(url);
            var allRates = responseResult["rates"];

            return allRates[RateConsts.HryvnyaCode.ToLower()];
        }
    }
}
