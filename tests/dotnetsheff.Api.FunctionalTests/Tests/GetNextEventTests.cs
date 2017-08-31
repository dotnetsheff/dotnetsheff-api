using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using HttpMock;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Ploeh.AutoFixture;
using Xunit;

namespace dotnetsheff.Api.FunctionalTests.Tests
{
    public class GetNextEventTests :IDisposable
    {
        private readonly AzureFunctionsStartup _azureFunctionsStartup;
        private readonly int _port;
        private readonly IHttpServer _stubHttp;
        private readonly Fixture _fixture;
        private readonly Event _expectedEvent;

        public GetNextEventTests()
        {
            _fixture = new Fixture();
            _expectedEvent = _fixture.Create<Event>();
            _expectedEvent.Time = _expectedEvent.Time.ToUniversalTime();

            var meetupApiBaseUri = "http://localhost:9191";
            var meetupApiKey = Guid.NewGuid().ToString();
            _stubHttp = HttpMockRepository.At(meetupApiBaseUri);
            _stubHttp.Stub(x => x.Get("/dotnetsheff/events"))
                .WithParams(new Dictionary<string, string>()
                {
                    {"apiKey" , meetupApiKey },
                    {"status" ,"upcoming" },
                    {"page" ,"1" },
                    {"omit" ,"created,status,updated,utc_offset,waitlist_count,venue,group,manual_attendance_count,visibility" },
                })
                .Return($@"[
    {{
        ""id"": ""{_expectedEvent.Id}"",
        ""name"": ""{_expectedEvent.Name}"",
        ""time"": {new DateTimeOffset(_expectedEvent.Time).ToUnixTimeMilliseconds()},
        ""yes_rsvp_count"": {_expectedEvent.YesRsvpCount},
        ""link"": ""{_expectedEvent.Link}"",
        ""description"": ""{_expectedEvent.Description}""
    }}
]")
                .OK();
            _stubHttp.Start();

            _port = 7075;
            _azureFunctionsStartup = new AzureFunctionsStartup(_port, meetupApiBaseUri, meetupApiKey);
            _azureFunctionsStartup.Start();         
        }

        [Fact]
        public async Task ShouldReturnResults()
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"http://localhost:{_port}/api/events/next");

                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadAsStringAsync();

                var @event = JsonConvert.DeserializeObject<Event>(result, new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

                @event.ShouldBeEquivalentTo(_expectedEvent,
                    c => c.Using(new DateTimeWithinOneMillisecondEquivalencyStep()));
            }
        }

        public void Dispose()
        {
            _stubHttp.Dispose();
            _azureFunctionsStartup?.Stop();
        }
    }
}
