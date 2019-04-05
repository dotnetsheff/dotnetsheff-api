using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
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
        private readonly HttpClient _client;
        private readonly CloudTable _eventFeedbackTable;

        public PostFeedbackEventTests()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(MeetupSettings.MeetupApiBaseUri)
            };
            _eventFeedbackTable = CloudStorageAccount.DevelopmentStorageAccount.CreateCloudTableClient().GetTableReference("eventfeedback");
        }

        [Fact]
        public async Task ShouldSaveFeedbackToStorage()
        {
            var url = $"http://localhost:{AzureFunctionsFixture.Port}/api/feedback";

            var expected = new EventFeedback
            {
                Id = "1",
                Title = ".net is dead",
                //Talks= new[]
                //{
                //    new TalkFeedback
                //    {
                //        Id = "1-1",
                //        Title = "Kev is shit",
                //    }, 
                //    new TalkFeedback
                //    {
                //        Id = "1-2",
                //        Title = "Kev is more shit"
                //    }
                //}
            };

            var response = await _client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(expected)));

            response.EnsureSuccessStatusCode();

            var actual = _eventFeedbackTable.CreateQuery<EventTableEntity>().Execute().SingleOrDefault(x => x.Id == expected.Id);

            actual.Should().NotBeNull();
            actual.Title.Should().Be(expected.Title);
        }
        
        public void Dispose()
        {
            _eventFeedbackTable.Delete();
            _client.Dispose();
        }
    }

    public class TalkFeedback
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Speaker { get; set; }
        public string Rating { get; set; }
        public string Enjoyed { get; set; }
        public string Improvements { get; set; }
    }

    public class EventFeedback
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Overall { get; set; }
        public string Food { get; set; }
        public string Drinks { get; set; }
        public string Venue { get; set; }
        public string Enjoyed { get; set; }
        public string Improvements { get; set; }
        public TalkFeedback[] Talks { get; set; }
    }

    public class EventTableEntity : TableEntity
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Overall { get; set; }
        public string Food { get; set; }
        public string Drinks { get; set; }
        public string Venue { get; set; }
        public string Enjoyed { get; set; }
        public string Improvements { get; set; }
        public TalkTableEntity[] Talks { get; set; }
    }

    public class TalkTableEntity : TableEntity
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Speaker { get; set; }
        public string Rating { get; set; }
        public string Enjoyed { get; set; }
        public string Improvements { get; set; }
    }
}
