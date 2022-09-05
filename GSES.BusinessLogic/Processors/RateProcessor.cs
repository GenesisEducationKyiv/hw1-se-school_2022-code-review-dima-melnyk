using GSES.BusinessLogic.Consts;
using GSES.BusinessLogic.Extensions;
using GSES.BusinessLogic.Models.Rate;
using GSES.BusinessLogic.Processors.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace GSES.BusinessLogic.Processors
{
    public class RateProcessor : IRateProcessor
    {
        private readonly HttpClient httpClient;
        private readonly IConfiguration configuration;

        public RateProcessor(HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;
            this.configuration = configuration;
        }

        public async Task<BaseRateModel> GetRateAsync()
        {
            this.httpClient.DefaultRequestHeaders.Add(RateConsts.KeyHeaderName, this.configuration[RateConsts.ConfigApiKey]);

            var url = string.Format(this.configuration[RateConsts.ApiUrlKey], RateConsts.BitcoinCode, RateConsts.HryvnyaCode);
            return await this.httpClient.GetModelFromRequest<CoinApiRateModel>(url);
        }
    }
}
