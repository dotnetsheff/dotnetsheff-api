using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using HttpMock;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Ploeh.AutoFixture;
using Xunit;
using System.Linq;

namespace dotnetsheff.Api.FunctionalTests.Tests.GetAvailableFeedbackEvents
{
    [Collection(XUnitCollectionNames.ApiCollection)]
    public class GetAvailableFeedbackEventsTests :  IDisposable
    {
        private readonly IHttpServer _stubHttp;

        public GetAvailableFeedbackEventsTests()
        {
            _stubHttp = HttpMockRepository.At(MeetupSettings.MeetupApiBaseUri);
            _stubHttp.Stub(x => x.Get("/dotnetsheff/events"))
                .WithParams(new Dictionary<string, string>()
                {
                    {"apiKey" , MeetupSettings.MeetupApiKey },
                    {"status" ,"upcoming" },
                    {"page" ,"1" },
                    {"omit" ,"created,status,updated,utc_offset,waitlist_count,venue,group,manual_attendance_count,visibility" },
                })
                .Return($@"[
    {{
        ""id"": ""254791420"",
        ""name"": ""Chocolatey with Gary Park and HTTP API patterns with Toby Henderson"",
        ""time"": {new DateTimeOffset(2019, 01, 28, 0, 0, 0, TimeSpan.Zero).ToUnixTimeMilliseconds()},
        ""yes_rsvp_count"": {10},
        ""link"": """",
        ""description"": ""This event will be split into two parts, Gary Ewan Park presenting Adding a layer of Chocolate(y) and the second half will be Toby Henderson presenting HTTP API patterns.""
    }}
]").OK();
            _stubHttp.Start();
        }

        [Fact]
        public async Task ShouldReturnResults()
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"http://localhost:{AzureFunctionsFixture.Port}/api/feedback/available-events");

                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadAsStringAsync();

                var events = JsonConvert.DeserializeObject<JObject[]>(result, new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

                var expected = JObject.Parse(@"{ ""id"": ""254791420"", ""title"": ""Chocolatey with Gary Park and HTTP API patterns with Toby Henderson"", ""date"": ""2019-01-28"", ""talks"": [ { ""title"": ""Adding a layer of Chocolate(y)"", ""speaker"": ""Gary Ewan Park"" }, { ""title"": ""HTTP API patterns"", ""speaker"": ""Toby Henderson"" } ] }");

                events.First().ShouldBeEquivalentTo(expected,
                    c => c.Using(new DateTimeWithinOneMillisecondEquivalencyStep()));
            }
        }

        public void Dispose()
        {
            _stubHttp.Dispose();
        }
    }
}
