using Microsoft.WindowsAzure.Storage.Table;

namespace dotnetsheff.Api.PostFeedbackEvent
{
    public class EventFeedbackTableEntity : TableEntity
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Overall { get; set; }
        public string Food { get; set; }
        public string Drinks { get; set; }
        public string Venue { get; set; }
        public string Enjoyed { get; set; }
        public string Improvements { get; set; }
        public TalkFeedbackTableEntity[] Talks { get; set; }
    }
}