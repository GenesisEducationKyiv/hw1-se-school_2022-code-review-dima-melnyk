using Newtonsoft.Json;
using Serilog;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace GSES.BusinessLogic.Extensions
{
    public static class HttpClientExtension
    {
        public async static Task<T> GetModelFromRequest<T>(this HttpClient httpClient, string url)
        {
            var uri = new Uri(url);

            var response = await httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();

            Log.Logger.Information($"{uri.Host}: {responseBody}");
            return JsonConvert.DeserializeObject<T>(responseBody);
        }
    }
}
