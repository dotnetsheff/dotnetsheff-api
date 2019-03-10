using System.IO;
using System.Linq;
using dotnetsheff.Api.GetAvailableFeedbackEvents;
using FluentAssertions;
using Xunit;

namespace dotnetsheff.Api.Tests
{
    public class TwoSpeakersTalkParserTests
    {
        [Fact]
        public void ShouldReturnCorrectTalksForTwoSpeakers()
        {
            var input = new PastEvent
            {
                Name = "Chocolatey with Gary Park and HTTP API patterns with Toby Henderson",
                Description = File.ReadAllText("twotalkstwospeakersdescription.txt")
            };
            var talks = new TwoSpeakersTalkParser().Parse(input).ToArray();

            talks.ShouldBeEquivalentTo(new[]{
                new {Title = "Adding a layer of Chocolate(y)", Speaker = "Gary Ewan Park"},
                new {Title = "HTTP API patterns", Speaker = "Toby Henderson"},
            });
        }
    }
}