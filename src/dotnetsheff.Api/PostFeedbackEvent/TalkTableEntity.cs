using Microsoft.WindowsAzure.Storage.Table;

namespace dotnetsheff.Api.PostFeedbackEvent
{
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