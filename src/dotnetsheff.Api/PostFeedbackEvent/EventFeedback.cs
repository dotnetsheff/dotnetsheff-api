namespace dotnetsheff.Api.PostFeedbackEvent
{
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
        public TalkFeedback[] Talks{ get; set; }
    }
}