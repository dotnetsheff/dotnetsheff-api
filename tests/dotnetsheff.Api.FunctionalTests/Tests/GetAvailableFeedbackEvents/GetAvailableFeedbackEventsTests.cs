using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using HttpMock;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Xunit;

namespace dotnetsheff.Api.FunctionalTests.Tests.GetAvailableFeedbackEvents
{
    [Collection(XUnitCollectionNames.ApiCollection)]
    public class GetAvailableFeedbackEventsTests :  IDisposable
    {
        private readonly IHttpServer _stubHttp;

        private const string EVENTS_RESPONSE =
            @"[{""name"": ""Chocolatey with Gary Park and HTTP API patterns with Toby Henderson"",""description"":""<p>This event will be split into two parts, Gary Ewan Park presenting Adding a layer of Chocolate(y) and the second half will be Toby Henderson presenting HTTP API patterns.</p>""},
{""name"": ""How to parse a file & Kotlin for the curious with Matt Ellis"",""description"":""<p>This event will be broken down into 2 talks, How to parse a file and Kotlin for the curious both being presented by Matt Ellis.</p>""} ]";

        public GetAvailableFeedbackEventsTests()
        {
            _stubHttp = HttpMockRepository.At(MeetupSettings.MeetupApiBaseUri);
            _stubHttp.Stub(x => x.Get("/dotnetsheff/events"))
                .WithParams(new Dictionary<string, string>()
                {
                    {"apiKey" , MeetupSettings.MeetupApiKey },
                    {"status" ,"past" },
                    {"page" ,"3" },
                    {"only" ,"name,description" },
                    {"desc" ,"true" },
                })
                .Return(EVENTS_RESPONSE).OK();
            _stubHttp.Start();
        }

        [Fact]
        public async Task ShouldReturnResults()
        {
            using (var httpClient = new HttpClient())
            {
                var url = $"http://localhost:{AzureFunctionsFixture.Port}/api/feedback/available-events";
                var response = await httpClient.GetAsync(url);

                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadAsStringAsync();

                var events = JsonConvert.DeserializeObject<JObject[]>(result, new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

                //var expected = JObject.Parse(@"{""Title"": ""Chocolatey with Gary Park and HTTP API patterns with Toby Henderson"", ""Talks"": [ { ""Title"": ""Adding a layer of Chocolate(y)"", ""Speaker"": ""Gary Ewan Park"" }, { ""Title"": ""HTTP API patterns"", ""Speaker"": ""Toby Henderson"" } ] }");
                var expectedArray = JArray.Parse(@"[{""Title"": ""Chocolatey with Gary Park and HTTP API patterns with Toby Henderson"", ""Talks"": [ { ""Title"": ""Adding a layer of Chocolate(y)"", ""Speaker"": ""Gary Ewan Park"" }, { ""Title"": ""HTTP API patterns"", ""Speaker"": ""Toby Henderson"" } ] },
{""Title"": ""How to parse a file & Kotlin for the curious with Matt Ellis"", ""Talks"":[{""Title"":""How to parse a file"", ""Speaker"":""Matt Ellis""},{""Title"":""Kotlin for the curious"", ""Speaker"":""Matt Ellis""}]}]");

                events.ShouldBeEquivalentTo(expectedArray);
            }
        }

        public void Dispose()
        {
            _stubHttp.Dispose();
        }
    }
}
