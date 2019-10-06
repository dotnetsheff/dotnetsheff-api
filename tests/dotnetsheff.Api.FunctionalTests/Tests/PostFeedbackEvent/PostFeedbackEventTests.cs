using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using dotnetsheff.Api.FunctionalTests.Tests.PostFeedbackEvent.Models;
using FluentAssertions;
using Microsoft.Azure.Cosmos.Table;
using Newtonsoft.Json;
using Xunit;

namespace dotnetsheff.Api.FunctionalTests.Tests.PostFeedbackEvent
{
    [Collection(XUnitCollectionNames.ApiCollection)]
    public class PostFeedbackEventTests : IDisposable
    {
        private const string EVENT_TABLE_REFERENCE = "eventfeedback";
        private const string TALK_TABLE_REFERENCE = "talkfeedback";
        private readonly HttpClient _client;
        private readonly CloudTableClient _cloudTableClient;

        public PostFeedbackEventTests()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(MeetupSettings.MeetupApiBaseUri)
            };

            _cloudTableClient = CloudStorageAccount.DevelopmentStorageAccount.CreateCloudTableClient();
        }

        [Fact]
        public async Task ShouldSaveFeedbackToStorage()
        {
            var url = $"http://localhost:{AzureFunctionsFixture.Port}/api/feedback";

            var expected = new Fixture().Build<EventFeedback>().Create(); 

            var response = await _client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(expected)))
                .ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            var eventEntity = GetActualStoredEvent(expected);

            var talks = GetActualStoredTalks(expected);

            eventEntity.ShouldBeEquivalentTo(expected, o => o.ExcludingMissingMembers());
            talks.ShouldBeEquivalentTo(expected.Talks, o => o.ExcludingMissingMembers());
        }

        private TalkTableEntity[] GetActualStoredTalks(EventFeedback expected)
        {
            var talkPartitionKeys = expected.Talks.Select(
                x => $"{expected.Id}-{x.Id}").ToArray();

            var talkFeedbackTable = _cloudTableClient.GetTableReference(TALK_TABLE_REFERENCE);
            var talks = talkFeedbackTable
                .CreateQuery<TalkTableEntity>()
                .Where(x => x.PartitionKey == talkPartitionKeys[0] || x.PartitionKey == talkPartitionKeys[1] ||
                            x.PartitionKey == talkPartitionKeys[2])
                .ToArray();

            return talks;
        }

        private EventTableEntity GetActualStoredEvent(EventFeedback expected)
        {
            var eventFeedbackTable = _cloudTableClient.GetTableReference(EVENT_TABLE_REFERENCE);

            var eventEntity = eventFeedbackTable
                .CreateQuery<EventTableEntity>()
                .Where(x => x.PartitionKey == expected.Id)
                .ToArray()
                .Single();

            return eventEntity;
        }

        public void Dispose()
        {
            var eventFeedbackTable = _cloudTableClient.GetTableReference(EVENT_TABLE_REFERENCE);
            var talkFeedbackTable = _cloudTableClient.GetTableReference(TALK_TABLE_REFERENCE);

            eventFeedbackTable.DeleteIfExists();
            talkFeedbackTable.DeleteIfExists();

            _client.Dispose();
        }
    }
}
