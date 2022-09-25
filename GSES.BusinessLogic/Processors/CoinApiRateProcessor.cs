using GSES.BusinessLogic.Consts;
using GSES.BusinessLogic.Extensions;
using GSES.BusinessLogic.Models.Rate;
using GSES.BusinessLogic.Processors.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace GSES.BusinessLogic.Processors
{
    public class CoinApiRateProcessor : BaseRateProcessor
    {
        private readonly HttpClient httpClient;
        private readonly IConfiguration configuration;

        public CoinApiRateProcessor(HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;
            this.configuration = configuration;
        }

        public override async Task<(BaseRateModel, bool)> GetRateAsync()
        {
            BaseRateModel rate = null;
            var isSuccess = true;

            try
            {
                this.httpClient.DefaultRequestHeaders.Add(RateConsts.KeyHeaderName, this.configuration[RateConsts.ConfigApiKey]);

                var url = string.Format(this.configuration[RateConsts.CoinApiUrlKey], RateConsts.BitcoinCode, RateConsts.HryvnyaCode);

                rate = await this.httpClient.GetModelFromRequest<CoinApiRateModel>(url);

                if (rate is null)
                {
                    throw new Exception("Rate is null, process was not successful");
                }
            }
            catch (Exception)
            {
                isSuccess = false;
            }

            return (rate, isSuccess);
        }
    }
}
