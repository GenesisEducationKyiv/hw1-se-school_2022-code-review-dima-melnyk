using GSES.BusinessLogic.Consts;
using GSES.BusinessLogic.Extensions;
using GSES.BusinessLogic.Models.Rate;
using GSES.BusinessLogic.Processors.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace GSES.BusinessLogic.Processors
{
    public class CoingeckoRateProcessor : BaseRateProcessor
    {
        private readonly HttpClient httpClient;
        private readonly IConfiguration configuration;

        public CoingeckoRateProcessor(HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;
            this.configuration = configuration;
        }

        public override async Task<(BaseRateModel, bool)> GetRateAsync()
        {
            var url = this.configuration[RateConsts.CoingeckoUrlKey];
            BaseRateModel rate = null;
            var isSuccess = true;

            try
            {
                var responseResult = await this.httpClient.GetModelFromRequest<Dictionary<string, Dictionary<string, CoingeckoRateModel>>>(url);
                var allRates = responseResult["rates"];
                rate = allRates[RateConsts.HryvnyaCode.ToLower()];

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
