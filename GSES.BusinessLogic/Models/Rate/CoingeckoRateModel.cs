using GSES.BusinessLogic.Consts;
using Newtonsoft.Json;

namespace GSES.BusinessLogic.Models.Rate
{
    public class CoingeckoRateModel : BaseRateModel
    {
        [JsonProperty(RateConsts.NameField)]
        public string Name { get; set; }

        [JsonProperty(RateConsts.ValueField)]
        public override double Rate { get; set; }
    }
}
