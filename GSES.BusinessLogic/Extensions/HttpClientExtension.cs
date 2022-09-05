using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace GSES.BusinessLogic.Extensions
{
    public static class HttpClientExtension
    {
        public async static Task<T> GetModelFromRequest<T>(this HttpClient httpClient, string url)
        {
            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseBody);
        }
    }
}
