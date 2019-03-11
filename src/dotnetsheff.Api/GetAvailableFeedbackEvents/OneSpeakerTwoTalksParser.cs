using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace dotnetsheff.Api.GetAvailableFeedbackEvents
{
    public class OneSpeakerTwoTalksParser : ITalkParser
    {
        private static readonly char[] CharactersToTrim = {' ', '\\', '.', '"'};

        public IEnumerable<Talk> Parse(PastEvent pastEvent)
        {
            var oneSpeakerRegex = new Regex("This event will be (split|broken down) into (2|two) (parts|talks), (?<talk1>.+) and (?<talk2>.+) both being presented by (?<speaker>.+?)</p>");

            var match = oneSpeakerRegex.Match(pastEvent.Description);

            if (!match.Success) yield break;

            yield return new Talk
            {
                Title = match.Groups["talk1"].Value.Trim(CharactersToTrim),
                Speaker = match.Groups["speaker"].Value.Trim(CharactersToTrim)
            };

            yield return new Talk
            {
                Title = match.Groups["talk2"].Value.Trim(CharactersToTrim),
                Speaker = match.Groups["speaker"].Value.Trim(CharactersToTrim)
            };
        }
    }
}