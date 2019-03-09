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
                Description = "This event will be split into two parts, Gary Ewan Park presenting Adding a layer of Chocolate(y) and the second half will be Toby Henderson presenting HTTP API patterns."
            };
            var talks = new TwoSpeakersTalkParser().Parse(input);

            talks.ShouldBeEquivalentTo(new[]{
                new {Title = "Adding a layer of Chocolate(y)", Speaker = "Gary Ewan Park"},
                new {Title = "HTTP API patterns", Speaker = "Toby Henderson"},
            });
        }
    }
}