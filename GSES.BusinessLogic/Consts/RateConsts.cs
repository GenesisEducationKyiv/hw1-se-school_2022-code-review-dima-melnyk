namespace GSES.BusinessLogic.Consts
{
    public static class RateConsts
    {
        public const string HttpDomain = "https://rest.coinapi.io/v1/exchangerate/{0}/{1}";

        public const string BitcoinCode = "BTC";

        public const string HryvnyaCode = "UAH";

        public const string KeyHeaderName = "X-CoinAPI-Key";

        public const string ConfigApiKey = "ApiKey";

        public const string TimeField = "time";

        public const string FromField = "asset_id_base";
     
        public const string ToField = "asset_id_quote";
    }
}
