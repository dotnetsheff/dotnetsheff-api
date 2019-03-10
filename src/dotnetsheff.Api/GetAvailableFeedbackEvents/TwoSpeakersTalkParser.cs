using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace dotnetsheff.Api.GetAvailableFeedbackEvents
{
    public class TwoSpeakersTalkParser : ITalkParser
    {
        private static string[] _patterns =
        {
            "This event will be split into two parts, (?<speaker1>.+) presenting (?<talk1>.+) and the second half will be (?<speaker2>.+) presenting (?<talk2>.+?)</p>",
            "This event will be broken down into 2 talks, (?<talk1>.+) and (?<talk2>.+) being presented by (?<speaker1>.+?) and (?<speaker2>.+?)</p>",
        }; 
        public IEnumerable<Talk> Parse(PastEvent @event)
        {
            var twoSpeakerMatch = FindMatch(@event.Description);

            if (!twoSpeakerMatch.Success) yield break;

            var speaker1 = twoSpeakerMatch.Groups["speaker1"].Value.Trim(' ','.');
            var speaker2 = twoSpeakerMatch.Groups["speaker2"].Value.Trim(' ','.');

            var talk1 = twoSpeakerMatch.Groups["talk1"].Value.Trim(' ','.');
            var talk2 = twoSpeakerMatch.Groups["talk2"].Value.Trim(' ','.');

            yield return new Talk { Title = talk1, Speaker = speaker1 };
            yield return new Talk { Title = talk2, Speaker = speaker2 };
        }

        private static Match FindMatch(string description)
        {
            foreach (var pattern in _patterns)
            {
                if (Regex.IsMatch(description, pattern))
                    return Regex.Match(description, pattern);
            }

            return Match.Empty;
        }
    }
}