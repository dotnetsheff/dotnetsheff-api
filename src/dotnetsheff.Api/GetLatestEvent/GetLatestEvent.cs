using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Data.Edm.Csdl;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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

            var nextEventQuery = QueryFactory.CreateNextEventQuery();

            var @event = await nextEventQuery.Execute();

            var convertor = new EventToNextEventConvertor(new EventDescriptionShortener());
            var nextEvent = convertor.Convert(@event);

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(nextEvent), Encoding.UTF8, "application/json")
            };
        }
    }
}
