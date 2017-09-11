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
    [Collection(XUnitCollectionNames.ApiCollection)]
    public class GetNextEventTests :  IDisposable
    {
        private readonly IHttpServer _stubHttp;
        private readonly Event _expectedEvent;

        public GetNextEventTests()
        {
            var fixture = new Fixture();
            _expectedEvent = fixture.Create<Event>();
            _expectedEvent.Time = _expectedEvent.Time.ToUniversalTime();

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
        ""id"": ""{_expectedEvent.Id}"",
        ""name"": ""{_expectedEvent.Name}"",
        ""time"": {new DateTimeOffset(_expectedEvent.Time).ToUnixTimeMilliseconds()},
        ""yes_rsvp_count"": {_expectedEvent.YesRsvpCount},
        ""link"": ""{_expectedEvent.Link}"",
        ""description"": ""{_expectedEvent.ShortDescription}""
    }}
]").OK();
            _stubHttp.Start();
        }

        [Fact]
        public async Task ShouldReturnResults()
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"http://localhost:{AzureFunctionsFixture.Port}/api/events/next");

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
        }
    }
}
