using System;
using System.Collections.Generic;
using System.IO;
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

        private const string EVENTS_RESPONSE = @"[{""description"": ""<p>This event will be broken down into 2 talks, Reasonable Software and A more flexible way to store your data with MongoDB being presented by Ian Johnson and Kevin Smith.</p> <p>📅 Agenda<br/>- 👋 Welcome<br/>- 🗑️ Housekeeping<br/>- 👩\u200d🏫 Session 1 - Reasonable Software<br/>- 🍕 Food/Drinks<br/>- 👩\u200d🏫 Session 2 - A more flexible way to store your data with MongoDB<br/>- 🍻 Social @ <a href=\""http://www.sheffieldtap.com/\"" class=\""linkified\"">http://www.sheffieldtap.com/</a></p> <p>*Reasonable Software*</p> <p>In a reasonable a system (i.e. a system that helps me to understand it, to reason about it) I should be able to understand how to make a change without holding the entire system in my head. I should be able to reason where the change needs to be made and reason about the impact it will have.</p> <p>I want to explore what reasonable means to me, from the processes of the team all the way down to an individual block of code. Along the way, we will encounter existing frameworks, tools, and patterns that our community has developed over the years to help us to reason about our code and processes; I feel that they have often been misused and end up creating the opposite effect, adding unnecessary complexity to how we work.</p> <p>*Ian Johnson*<br/>Ian is a software developer working at Redgate, a company that develops tools for developers and database administrators.</p> <p>Ian is passionate about writing maintainable code that delivers on the needs of users. Though he considers himself an introvert, Ian loves talking with other developers, learning from their experiences and sharing his own.</p> <p>Outside of work, Ian is a passionate Star Wars fan and has been known to make the occasional really bad pun, but all of his code is \""no-pun sourced\"" (sorry, couldn't resist).</p> <p><a href=\""http://blog.ninjaferret.co.uk\"" class=\""linkified\"">http://blog.ninjaferret.co.uk</a></p> <p>@ijohnson_tnf</p> <p>*A more flexible way to store your data with MongoDB*</p> <p>If you've been anywhere near software development, the norm is to store your data in a relational form, but what if there was a different way</p> <p>We will take a look at the history of MongoDB and why it continues to be a trending database year on year. We will then go into the advantages of having a flexible document model and how we can utilize MongoDB for our application storage.</p> <p>*Kevin Smith*<br/>He runs dotnet York and dotnetsheff, and casually speaks at user groups. He is always keen to contribute to open source projects, one of the main ones being MassTransit. He has worked across a broad range of domains including: law, travel, finance and analytics.</p> <p><a href=\""https://kevsoft.net\"" class=\""linkified\"">https://kevsoft.net</a></p> <p>@kev_bite</p> "",""name"": ""Reasonable Software with Ian Johnson and MongoDB with Kevin Smith"",""id"": ""254554781""}]";


        public GetAvailableFeedbackEventsTests()
        {
            _stubHttp = HttpMockRepository.At(MeetupSettings.MeetupApiBaseUri);
            _stubHttp.Stub(x => x.Get("/dotnetsheff/events"))
                .WithParams(new Dictionary<string, string>()
                {
                    {"apiKey" , MeetupSettings.MeetupApiKey },
                    {"status" ,"past" },
                    {"page" ,"3" },
                    {"only" ,"id,name,description" },
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

                var expectedArray = JArray.Parse(@"
[{""Id"":""254554781"", ""Title"": ""Reasonable Software with Ian Johnson and MongoDB with Kevin Smith"", ""Talks"": [ { ""Title"": ""Reasonable Software"", ""Speaker"": ""Ian Johnson"" }, { ""Title"": ""A more flexible way to store your data with MongoDB"", ""Speaker"": ""Kevin Smith""}]}]");

                events.ShouldBeEquivalentTo(expectedArray);
            }
        }

        public void Dispose()
        {
            _stubHttp.Dispose();
        }
    }
}
