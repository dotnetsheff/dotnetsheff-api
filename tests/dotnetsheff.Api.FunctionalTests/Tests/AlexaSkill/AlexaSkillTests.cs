using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using HttpMock;
using Newtonsoft.Json.Linq;
using Ploeh.AutoFixture;
using Xunit;

namespace dotnetsheff.Api.FunctionalTests.Tests.AlexaSkill
{

    [Collection(XUnitCollectionNames.ApiCollection)]
    public class AlexaSkillTests : IDisposable
    {
        private readonly Event _expectedEvent;
        private readonly IHttpServer _stubHttp;

        public AlexaSkillTests()
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
                    {"omit" ,"created,status,updated,utc_offset,waitlist_count,venue,group,manual_attendance_count,visibility,yes_rsvp_count,link,description" },
                })
                .Return($@"[
    {{
        ""id"": ""{_expectedEvent.Id}"",
        ""name"": ""{_expectedEvent.Name}"",
        ""time"": {new DateTimeOffset(_expectedEvent.Time).ToUnixTimeMilliseconds()}
    }}
]").OK();
            _stubHttp.Start();
        }

        [Fact]
        public async Task ShouldReturnSpeechAndEndSession()
        {
            using (var httpClient = new HttpClient())
            {
                var alexaRequest = BuildAlexaRequest(AlexaSettings.SkillId, new Guid().ToString(), "GetNextEvent");
                var content = new StringContent(alexaRequest, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync($"http://localhost:{AzureFunctionsFixture.Port}/api/alexa-skill", content);

                response.EnsureSuccessStatusCode();
                    
                var result = await response.Content.ReadAsStringAsync();

                var jObject = JObject.Parse(result);

                jObject["response"]["outputSpeech"]
                    .HasValues.Should().BeTrue();

                jObject["response"]["shouldEndSession"]
                    .Value<bool>().Should().BeTrue();
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
    ""timestamp"": ""{DateTime.UtcNow:O}""
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
        ""userId"": ""amzn1.ask.account.AAMAONSIFHEKEVSOFTISAWESOMEFNSOSNRI39FNGKNDFKJKSDGLKNLDNFGFJNGI3049RG9ERGENIDOFGNDFGFNOEA6TPUWY2Z3HTHM6NWH34CTZBQEBYSZUNAL2AW4GJELH7D6BK7QCJC5TD7ISHBOCWVR3F22HEYUMKQGQK6MNJY6VBROKNLENJUYDKF247ZM7ZWBXINPB5X4A""
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
            _stubHttp?.Dispose();
        }
    }
}
