using FluentAssertions;
using GSES.BusinessLogic.Consts;
using GSES.BusinessLogic.Processors;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GSES.BusinessLogic.Tests.Processors
{
    public class RateProcessorTests
    {
        [Fact]
        public async Task GetRateAsync_ReturnsRateSuccessfully()
        {
            // Assert
            var configMock = new Mock<IConfiguration>();
            configMock.Setup(c => c[RateConsts.ApiUrlKey]).Returns("https://google.com");

            var apiResponse = @"{""time"": ""2020-01-01T00:00:00.0000000Z"",
                  ""asset_id_base"": ""BTC"",
                  ""asset_id_quote"": ""USD"",
                  ""rate"": 3000
                }";
            var httpResponseHandlerMock = new HttpMessageHandlerMock() { Content = apiResponse };
            var httpClient = new HttpClient(httpResponseHandlerMock);

            var rateProcessor = new RateProcessor(httpClient, configMock.Object);

            // Act
            var result = await rateProcessor.GetRateAsync();

            // Assert
            result.Rate.Should().Be(3000);
        }
    }

    public class HttpMessageHandlerMock : HttpMessageHandler
    {
        public string Content { get; set; }
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new HttpResponseMessage() { Content = new StringContent(Content) });
        }

    }
}
