using GSES.BusinessLogic.Consts;
using Newtonsoft.Json;

namespace GSES.BusinessLogic.Models.Rate
{
    [JsonObject]
    public class CoinApiRateModel : BaseRateModel
    {
        [JsonProperty(RateConsts.TimeField)]
        public string Time { get; set; }

        [JsonProperty(RateConsts.FromField)]
        public string From { get; set; }

        [JsonProperty(RateConsts.ToField)]
        public string To { get; set; }
    }
}
