using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace dotnetsheff.Api.PostFeedbackEvent
{
    public class PostFeedbackEvent
    {
        private static TraceWriter _log;
        private static IAsyncCollector<EventFeedbackTableEntity> _eventCollector;
        private static IAsyncCollector<TalkFeedbackTableEntity> _talkCollector;

        [FunctionName("PostFeedbackEvent")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "feedback")] HttpRequestMessage request,
            [Table("eventfeedback", Connection = "AzureWebJobsStorage")] IAsyncCollector<EventFeedbackTableEntity> eventCollector,
            [Table("talkfeedback", Connection = "AzureWebJobsStorage")] IAsyncCollector<TalkFeedbackTableEntity> talkCollector,
            TraceWriter log)
        {
            _log = log;
            _eventCollector = eventCollector;
            _talkCollector = talkCollector;

            var eventTableEntity = await GetTableEntity(request);

            await SaveToAzureStorage(eventTableEntity);

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("Hello", Encoding.UTF8, "application/text")
            };
        }

        private static async Task SaveToAzureStorage(
            EventFeedbackTableEntity eventTableEntity)
        {
            _log.Info($"eventId: {eventTableEntity.Id} partitionKey: {eventTableEntity.PartitionKey} rowKey:{eventTableEntity.RowKey}");

            await _eventCollector.AddAsync(eventTableEntity);

            _log.Info($"Event with rowkey: {eventTableEntity.Id} and partitionKey: {eventTableEntity.PartitionKey} was added.");

            foreach (var talk in eventTableEntity.Talks)
            {
                talk.PartitionKey = eventTableEntity.Id;
                await _talkCollector.AddAsync(talk);
                _log.Info($"Talk with rowkey: {talk.Id} and partitionKey: {talk.PartitionKey} was added.");
            }
        }

        private static async Task<EventFeedbackTableEntity> GetTableEntity(HttpRequestMessage request)
        {
            _log.Info("Request for event feedback received.");

            var eventFeedback = JsonConvert.DeserializeObject<EventFeedback>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

            _log.Info($"Event feedback for: '{eventFeedback.Title}' extracted.");

            return Container.Instance.Resolve<IMapper>(_log).Map<EventFeedbackTableEntity>(eventFeedback);
        }
    }
}
