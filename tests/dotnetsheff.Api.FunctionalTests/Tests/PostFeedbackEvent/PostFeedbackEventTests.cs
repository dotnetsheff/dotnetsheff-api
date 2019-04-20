using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using dotnetsheff.Api.FunctionalTests.Tests.PostFeedbackEvent.Models;
using FluentAssertions;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
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
        private readonly CloudTable _eventFeedbackTable;
        private readonly CloudTable _talkFeedbackTable;

        public PostFeedbackEventTests()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(MeetupSettings.MeetupApiBaseUri)
            };
            _eventFeedbackTable = CloudStorageAccount.DevelopmentStorageAccount.CreateCloudTableClient().GetTableReference(EVENT_TABLE_REFERENCE);
            _talkFeedbackTable = CloudStorageAccount.DevelopmentStorageAccount.CreateCloudTableClient().GetTableReference(TALK_TABLE_REFERENCE);
        }

        [Fact]
        public async Task ShouldSaveFeedbackToStorage()
        {
            var url = $"http://localhost:{AzureFunctionsFixture.Port}/api/feedback";

            var expected = new Fixture().Build<EventFeedback>().Create(); 

            var response = await _client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(expected)));

            response.EnsureSuccessStatusCode();

            var eventEntity = _eventFeedbackTable
                .CreateQuery<EventTableEntity>()
                .Execute()
                .SingleOrDefault(x => x.Id == expected.Id);
            var talks = _talkFeedbackTable
                .CreateQuery<TalkTableEntity>()
                .Execute()
                .Where(x => x.PartitionKey == eventEntity?.Id);

            eventEntity.ShouldBeEquivalentTo(expected, o => o.ExcludingMissingMembers().Excluding(x => x.Talks));
            talks.ShouldBeEquivalentTo(expected.Talks, o => o.ExcludingMissingMembers());
        }
        
        public void Dispose()
        {
            _eventFeedbackTable.Delete();
            _client.Dispose();
        }
    }
}
