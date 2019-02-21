using System.Collections.Generic;

namespace dotnetsheff.Api.GetAvailableFeedbackEvents
{
    public interface ITalkParser
    {
        IEnumerable<Talk> Parse(PastEvent pastEvent);
    }
}