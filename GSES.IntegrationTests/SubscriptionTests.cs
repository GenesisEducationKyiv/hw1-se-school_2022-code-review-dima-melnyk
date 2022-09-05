using FluentAssertions;
using GSES.API;
using GSES.DataAccess.Consts;
using GSES.DataAccess.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace GSES.IntegrationTests
{
    public class SubscriptionTests : IDisposable
    {
        private readonly TestServer server;

        public SubscriptionTests()
        {
            this.server = new TestServer(new WebHostBuilder()
                .ConfigureAppConfiguration((context, config) => config.AddJsonFile("testsettings.json"))
                .UseStartup<Startup>());
        }

        public void Dispose()
        {
            var filePath = ((IConfiguration)this.server.Services.GetService(typeof(IConfiguration)))["DataPath"];
            var dir = new DirectoryInfo(filePath);
            dir.Attributes &= ~FileAttributes.ReadOnly;
            dir.Delete(true);

            this.server.Dispose();
        }

        [Fact]
        public async Task Subscribe_AddsRecordSuccessfully()
        {
            // Arrange
            var client = server.CreateClient();

            var email = "test@email.com";
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"/subscribe?email={email}");

            // Act
            var response = await client.SendAsync(httpRequest);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            (await IsInTestFolder(email)).Should().Be(true);
        }

        [Fact]
        public async Task Subscribe_Duplicate_ShowsDuplicateCode()
        {
            // Arrange
            var client = server.CreateClient();

            var email = "test@email.com";
            var httpRequest1 = new HttpRequestMessage(HttpMethod.Post, $"/subscribe?email={email}");
            var httpRequest2 = new HttpRequestMessage(HttpMethod.Post, $"/subscribe?email={email}");

            // Act
            var response = await client.SendAsync(httpRequest1);
            var response2 = await client.SendAsync(httpRequest2);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            response2.StatusCode.Should().Be(System.Net.HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task Subscribe_InvalidEmail_ShowsBadRequestCode()
        {
            // Arrange
            var client = server.CreateClient();

            var email = "test";
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"/subscribe?email={email}");

            // Act
            var response = await client.SendAsync(httpRequest);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        private async Task<bool> IsInTestFolder(string email)
        {
            var filePath = ((IConfiguration)this.server.Services.GetService(typeof(IConfiguration)))["DataPath"];
            var serializedElements = await File.ReadAllTextAsync(filePath + typeof(Subscriber) + GeneralConsts.JsonExtension);
            var elements = JsonConvert.DeserializeObject<IEnumerable<Subscriber>>(serializedElements);

            return elements.Contains(new Subscriber { Email = email });
        }
    }
}
