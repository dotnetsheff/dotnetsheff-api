using System.Collections.Generic;

namespace dotnetsheff.Api.GetAvailableFeedbackEvents
{
    public interface ITalkParser
    {
        IEnumerable<Talk> Parse(Event @event);
    }

    public class Talk
    {
        public string Title { get; set; }
        public string Speaker { get; set; }
    }
}