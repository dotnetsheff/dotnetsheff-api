using Microsoft.Azure.Cosmos.Table;

namespace dotnetsheff.Api.FunctionalTests.Tests.PostFeedbackEvent.Models
{
    internal class EventTableEntity : TableEntity
    {
        public string Title { get; set; }
        public string Overall { get; set; }
        public string Food { get; set; }
        public string Drinks { get; set; }
        public string Venue { get; set; }
        public string Enjoyed { get; set; }
        public string Improvements { get; set; }
    }
}