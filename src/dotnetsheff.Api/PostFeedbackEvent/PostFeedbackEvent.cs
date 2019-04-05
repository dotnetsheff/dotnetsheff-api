using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace dotnetsheff.Api.PostFeedbackEvent
{
    public class PostFeedbackEvent
    {
        [FunctionName("PostFeedbackEvent")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "feedback")] HttpRequestMessage request,
            [Table("eventfeedback", Connection = "AzureWebJobsStorage")] IAsyncCollector<EventTableEntity> eventFeedback,
            TraceWriter log)
        {
            log.Info("Posting feedback for event");

            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            var req = JsonConvert.DeserializeObject<EventFeedback>(await request.Content.ReadAsStringAsync(), settings);

            log.Info($"Event {req.Title}");

            Mapper.Initialize(cfg => cfg.AddProfile<EventFeedbackProfile>());
            var eventTableEntity = Mapper.Map<EventTableEntity>(req);

            log.Info($"eventId: {eventTableEntity.Id} partitionKey: {eventTableEntity.PartitionKey} rowKey:{eventTableEntity.RowKey}");
            await eventFeedback.AddAsync(eventTableEntity);

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("Hello", Encoding.UTF8, "application/text")
            };
        }
    }
}
