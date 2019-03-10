using System.Collections.Generic;
using System.IO;
using System.Linq;
using dotnetsheff.Api.GetAvailableFeedbackEvents;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace dotnetsheff.Api.Tests
{
    public class TwoSpeakersTalkParserTests
    {
        [Theory]
        [MemberData(nameof(TwoSpeakersCases))]
        public void ShouldReturnCorrectTalksForTwoSpeakers(PastEvent pastEvent, IEnumerable<Talk> expectedTalks) => 
            new TwoSpeakersTalkParser().Parse(pastEvent).ShouldBeEquivalentTo(expectedTalks);

        public static IEnumerable<object[]> TwoSpeakersCases =>
            new List<object[]>
            {
                new object[]
                {
                    JsonConvert.DeserializeObject<PastEvent[]>(File.ReadAllText("twotalkstwospeakersdescription.txt")).First(),
                    new[]
                    {
                        new Talk
                        {
                            Title = "Reasonable Software",
                            Speaker = "Ian Johnson"
                        },
                        new Talk
                        {
                            Title = "A more flexible way to store your data with MongoDB",
                            Speaker = "Kevin Smith"
                        }
                    }
                },
                new object[]
                {
                    JsonConvert.DeserializeObject<PastEvent[]>(File.ReadAllText("twotalkstwospeakersdescription.txt")).Last(),
                    new[]
                    {
                        new Talk
                        {
                            Title = "Adding a layer of Chocolate(y)",
                            Speaker = "Gary Ewan Park"
                        },
                        new Talk
                        {
                            Title = "HTTP API patterns",
                            Speaker = "Toby Henderson"
                        }
                    }
                },
            };
    }
}