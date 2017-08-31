using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using dotnetsheff.Api.Meetup;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace dotnetsheff.Api.GetLatestEvent
{
    public class GetLatestEvent
    {
        [FunctionName("GetNextEvent")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "events/next")] HttpRequestMessage req,
            TraceWriter log)
        {
            log.Info("Getting next event");
            var meetupSettings = new MeetupSettings();
            var api = new MeetupApiFactory(new MeetupSettings()).Create();
            var nextEventQuery = new NextEventQuery(api, "dotnetsheff", meetupSettings.ApiKey);

            var @event = await nextEventQuery.Execute();

            return req.CreateResponse(HttpStatusCode.OK, @event);

        }
    }
}
