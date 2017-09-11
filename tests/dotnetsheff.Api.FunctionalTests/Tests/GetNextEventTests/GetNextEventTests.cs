using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using HttpMock;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Ploeh.AutoFixture;
using Xunit;

namespace dotnetsheff.Api.FunctionalTests.Tests.GetNextEventTests
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
                string applicationId = Guid.NewGuid().ToString();
                var content = new StringContent(BuildAlexaRequest(applicationId, new Guid().ToString(), "GetNextEvent"), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync($"http://localhost:{AzureFunctionsFixture.Port}/api/events/next", content);

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

        public string BuildAlexaRequest(string applicationId, string sessionId, string intentName)
        {
            var json = $@"{{
  ""session"": {{
    ""new"": true,
    ""sessionId"": ""SessionId.{sessionId}"",
    ""application"": {{
      ""applicationId"": ""{applicationId}""
    }},
    ""attributes"": {{}},
    ""user"": {{
      ""userId"": ""amzn1.ask.account.AAMAONSIFHEKEVSOFTISAWESOMEFNSOSNRI39FNGKNDFKJKSDGLKNLDNFGFJNGI3049RG9ERGENIDOFGNDFGFNOEA6TPUWY2Z3HTHM6NWH34CTZBQEBYSZUNAL2AW4GJELH7D6BK7QCJC5TD7ISHBOCWVR3F22HEYUMKQGQK6MNJY6VBROKNLENJUYDKF247ZM7ZWBXINPB5X4A""
    }}
  }},
  ""request"": {{
    ""type"": ""IntentRequest"",
    ""requestId"": ""EdwRequestId.a071a50a-ba30-4d80-a5f9-606445d8a8ca"",
    ""intent"": {{
      ""name"": ""{intentName}"",
      ""slots"": {{}}
    }},
    ""locale"": ""en-GB"",
    ""timestamp"": ""2017-09-10T14:24:39Z""
  }},
  ""context"": {{
    ""AudioPlayer"": {{
      ""playerActivity"": ""IDLE""
    }},
    ""System"": {{
      ""application"": {{
        ""applicationId"": ""{applicationId}""
      }},
      ""user"": {{
        ""userId"": ""amzn1.ask.account.AEZ2YYUKVTAFL6ZNJ4QT2E2KGXKCMATONC6DQQZMPQOGLO2XL5C64H724KFJG7CHT5TRU5NKKOSXCDF4AH23R6OEA6TPUWY2Z3HTHM6NWH34CTZBQEBYSZUNAL2AW4GJELH7D6BK7QCJC5TD7ISHBOCWVR3F22HEYUMKQGQK6MNJY6VBROKNLENJUYDKF247ZM7ZWBXINPB5X4A""
      }},
      ""device"": {{
        ""supportedInterfaces"": {{}}
      }}
    }}
  }},
  ""version"": ""1.0""
}}";
            return json;
        }

        public void Dispose()
        {
            _stubHttp.Dispose();
        }
    }
}
