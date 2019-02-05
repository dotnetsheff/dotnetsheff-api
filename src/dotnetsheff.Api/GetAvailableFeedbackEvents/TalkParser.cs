using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace dotnetsheff.Api.GetAvailableFeedbackEvents
{
    public class TalkParser : ITalkParser
    {
        public IEnumerable<Talk> Parse(Event @event)
        {
            var twoSpeakersRegex = new Regex("This event will be split into two parts, (?<speaker1>.+) presenting (?<talk1>.+) and the second half will be (?<speaker2>.+) presenting (?<talk2>.+).");

            var twoSpeakerMatch = twoSpeakersRegex.Match(@event.Description);

            if (!twoSpeakerMatch.Success) throw new NotImplementedException();

            var speaker1 = twoSpeakerMatch.Groups["speaker1"].Value;
            var speaker2 = twoSpeakerMatch.Groups["speaker2"].Value;

            var talk1 = twoSpeakerMatch.Groups["talk1"].Value;
            var talk2 = twoSpeakerMatch.Groups["talk2"].Value;

            yield return new Talk { Title = talk1, Speaker = speaker1 };
            yield return new Talk { Title = talk2, Speaker = speaker2 };
        }
    }
}