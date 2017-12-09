using FileComparer;
using FluentAssertions;
using Interfaces.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Integrations.Tests
{

    [TestClass]
    public class ApiTests
    {
        private TestServer _server;
        private HttpClient _client;

        [TestInitialize]
        public void Initialize()
        {
            _server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            _client = _server.CreateClient();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _client.Dispose();
            _server.Dispose();
        }

        [TestMethod]
        public async Task WHEN_requesting_files_THEN_returns_empty_collection()
        {
            // Act
            var response = await _client.GetAsync("api/v1/files");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseStr = await response.Content.ReadAsStringAsync();
            var files = JsonConvert.DeserializeObject<TextFile[]>(responseStr);
            files.Should().BeEmpty();
        }

        [TestMethod]
        public async Task WHEN_posting_a_file_and_then_fetchin_THEN_returns_a_collection_with_the_file()
        {
            var file = new TextFile()
            {
                Id = Guid.NewGuid().ToString(),
                Data = "data"
            };
            
            // Act
            var postResponce = await _client.PostFileAsync(file);
            var getResponce = await _client.GetAsync("api/v1/files");

            // Assert
            postResponce.StatusCode.Should().Be(HttpStatusCode.Created);
            getResponce.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseStr = await getResponce.Content.ReadAsStringAsync();
            var files = JsonConvert.DeserializeObject<TextFile[]>(responseStr);
            files.Should().OnlyContain(f => f.Id == file.Id);
        }

        [TestMethod]
        public async Task WHEN_posting_identical_files_AND_comparing_them_THEN_returns_result_telling_files_are_unindentical()
        {
            var file = new TextFile()
            {
                Id = Guid.NewGuid().ToString(),
                Data = "data"
            };

            var file2 = new TextFile()
            {
                Id = Guid.NewGuid().ToString(),
                Data = "data"
            };

            // Act
            await Task.WhenAll(
                _client.PostFileAsync(file),
                _client.PostFileAsync(file2));

            var diffResponce = await _client.GetAsync($"api/v1/diff/right/{file.Id}/left/{file2.Id}");

            // Assert
            diffResponce.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseStr = await diffResponce.Content.ReadAsStringAsync();

            // Using dynamic here FileDiff is not deserializable
            // should use separate dtos if would want to support .net client
            dynamic diff = JObject.Parse(responseStr);
            string status = diff.status;
            status.Should().Be("Equal");
        }

        [TestMethod]
        public async Task WHEN_posting_files_with_different_content_AND_comparing_them_THEN_returns_result_telling_files_are_unindentical()
        {
            var file = new TextFile()
            {
                Id = Guid.NewGuid().ToString(),
                Data = 
@"{
    'name':'John',
    'age':30,
    'cars':[ 'Ford', 'BMW', 'Fiat' ]
    'motto': 'Value for live!'
    }"
            };

            var file2 = new TextFile()
            {
                Id = Guid.NewGuid().ToString(),
                Data =
@"{
    'name':'John',
    'cars':[ 'Ford', 'Fiat' ],
    'Address':'Homestreet 123',
  }"
            };

            // Act
            await Task.WhenAll(
                _client.PostFileAsync(file),
                _client.PostFileAsync(file2));

            var diffResponce = await _client.GetAsync($"api/v1/diff/right/{file.Id}/left/{file2.Id}");

            // Assert
            diffResponce.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseStr = await diffResponce.Content.ReadAsStringAsync();

            // Using dynamic here FileDiff is not deserializable
            // should use separate dtos if would want to support .net client
            dynamic diff = JObject.Parse(responseStr);
            string status = diff.status;
            status.Should().Be("NotEqual");   
        }
    }
}
