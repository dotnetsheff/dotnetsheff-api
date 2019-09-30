using System.Collections.Generic;
using System.Net;
using System.Net.Http;
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
        private readonly TraceWriter _log;
        private readonly IAsyncCollector<EventFeedbackTableEntity> _eventCollector;
        private readonly IAsyncCollector<TalkFeedbackTableEntity> _talkCollector;
        private readonly IMapper _mapper;

        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public PostFeedbackEvent(TraceWriter log, IAsyncCollector<EventFeedbackTableEntity> eventCollector, IAsyncCollector<TalkFeedbackTableEntity> talkCollector)
        {
            _log = log;
            _eventCollector = eventCollector;
            _talkCollector = talkCollector;
            _mapper = Container.Instance.Resolve<IMapper>(_log);
        }

        [FunctionName("PostFeedbackEvent")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "feedback")] HttpRequestMessage request,
            [Table("eventfeedback", Connection = "AzureWebJobsStorage")] IAsyncCollector<EventFeedbackTableEntity> eventCollector,
            [Table("talkfeedback", Connection = "AzureWebJobsStorage")] IAsyncCollector<TalkFeedbackTableEntity> talkCollector,
            TraceWriter log)
        {
            var postFeedbackEvent = new PostFeedbackEvent(log, eventCollector, talkCollector);

            await postFeedbackEvent.RunAsync(request)
                .ConfigureAwait(false);

            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }

        private async Task RunAsync(HttpRequestMessage request)
        {
            var eventTableEntity = await GetTableEntity(request)
                .ConfigureAwait(false);

            await SaveToAzureStorage(eventTableEntity.@event, eventTableEntity.talks)
                .ConfigureAwait(false);
        }

        private async Task SaveToAzureStorage(
            EventFeedbackTableEntity @event, IReadOnlyCollection<TalkFeedbackTableEntity> talks)
        {
            await _eventCollector.AddAsync(@event)
                .ConfigureAwait(false);

            _log.Info($"Event with partition key: {@event.PartitionKey} and row key: {@event.RowKey} was added.");

            foreach (var talk in talks)
            {
                await _talkCollector.AddAsync(talk)
                    .ConfigureAwait(false);

                _log.Info($"Talk with partition key: {talk.PartitionKey} and row key: {talk.RowKey} was added.");
            }
        }

        private async Task<(EventFeedbackTableEntity @event, IReadOnlyCollection<TalkFeedbackTableEntity> talks)> GetTableEntity(HttpRequestMessage request)
        {
            var eventFeedback = await GetEventFeedback(request);

            var @event = _mapper.Map<EventFeedbackTableEntity>(eventFeedback);
            var talks = _mapper.Map<TalkFeedbackTableEntity[]>(eventFeedback.Talks, opts => opts.Items.Add("EventId", eventFeedback.Id));

            return (@event, talks);
        }

        private async Task<EventFeedback> GetEventFeedback(HttpRequestMessage request)
        {
            _log.Info("Request for event feedback received.");

            var body = await request.Content.ReadAsStringAsync()
                .ConfigureAwait(false);

            var eventFeedback = JsonConvert.DeserializeObject<EventFeedback>(
                body,
                JsonSerializerSettings);

            _log.Info($"Event feedback for: '{eventFeedback.Title}' extracted.");

            return eventFeedback;
        }
    }
}
