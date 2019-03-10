using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace dotnetsheff.Api.GetAvailableFeedbackEvents
{
    public class OneSpeakerOneTalksParser : ITalkParser
    {
        private static readonly char[] CharactersToTrim = {' ', '\\', '.', '"'};

        public IEnumerable<Talk> Parse(PastEvent pastEvent)
        {
            var oneSpeakerRegex = new Regex("This event will be a single talk about (?<title>.+?) presented by (?<name>.+?)</p>");

            var match = oneSpeakerRegex.Match(pastEvent.Description);

            if (!match.Success) yield break;

            yield return new Talk
            {
                Title = match.Groups["title"].Value.Trim(CharactersToTrim),
                Speaker = match.Groups["name"].Value.Trim(CharactersToTrim)
            };
        }
    }
}