using System.IO;
using System.Linq;
using dotnetsheff.Api.GetAvailableFeedbackEvents;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace dotnetsheff.Api.Tests.GetAvailableFeedbackEvents.TalksParsersTests
{
    public class OneSpeakerTwoTalksParserTests
    {
        [Fact]
        public void ShouldReturnCorrectTalksForOneSpeaker()
        {
            var input = JsonConvert.DeserializeObject<PastEvent[]>(File.ReadAllText("onespeakertwotalksdescription.txt")).First();
            new OneSpeakerTwoTalksParser().Parse(input)
                .ShouldBeEquivalentTo(new[]
                {
                    new
                    {
                        Title = "How to parse a file", 
                        Speaker = "Matt Ellis"
                    },
                    new
                    {
                        Title = "Kotlin for the curious", 
                        Speaker = "Matt Ellis"
                    },
            });
        }
    }
}