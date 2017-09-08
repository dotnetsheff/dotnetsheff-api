using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using dotnetsheff.Api.GetLatestEvent;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace dotnetsheff.Api.AlexaSkill
{
    public static class AlexaSkill
    {
        [FunctionName("AlexaSkill")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "alexa-skill")]HttpRequestMessage req, TraceWriter log)
        {
            var skillRequest = JsonConvert.DeserializeObject<SkillRequest>(await req.Content.ReadAsStringAsync());

            if (skillRequest.Session.Application.ApplicationId != Environment.GetEnvironmentVariable("AlexaSkillId"))
            {
                var message = $"Unknown alex skill application. id = '{skillRequest.Session.Application.ApplicationId}'";
                log.Warning(message);

                return new HttpResponseMessage(HttpStatusCode.Unauthorized)
                {
                    Content = new StringContent(message)
                };
            }

            if (skillRequest.Request is IntentRequest intentRequest)
            {
                if (intentRequest.Intent.Name.Equals("GetNextEvent"))
                {
                    var alexaSkillEventQuery = Container.Instance.Resolve<IAlexaSkillEventQuery>();
                    var @event = await alexaSkillEventQuery.Execute();
                    var speech = new SsmlOutputSpeech();
                    speech.Ssml = $"<speak>The next dotnetsheff is <say-as interpret-as=\"date\">{@event.Time.Date:yyyyMMdd}</say-as>.";

                    var finalResponse = ResponseBuilder.Tell(speech);

                    return new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(finalResponse), Encoding.UTF8, "application/json")
                    };
                }
            }
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(ResponseBuilder.Empty()), Encoding.UTF8, "application/json")
            };
        }
    }
}
