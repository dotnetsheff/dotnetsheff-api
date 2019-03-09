using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace dotnetsheff.Api.GetAvailableFeedbackEvents
{
    public class LightningTalksParser : ITalkParser
    {
        private const string SLOT_PATTERN = @"Slot \d - (?'name'.*?) - (?'title'.*?)</p>";
        
        public IEnumerable<Talk> Parse(PastEvent pastEvent)
        {
            if (!Regex.IsMatch(pastEvent.Description, SLOT_PATTERN)) 
                return Enumerable.Empty<Talk>();

            return Regex.Matches(pastEvent.Description, SLOT_PATTERN)
                .OfType<Match>()
                .Select(slot => new Talk
                {
                    Title = slot.Groups["title"].Value.Trim(),
                    Speaker = Regex.Replace(slot.Groups["name"].Value, @"(\(+.*?\)+)", string.Empty).Trim()
                });
        }
    }
}
