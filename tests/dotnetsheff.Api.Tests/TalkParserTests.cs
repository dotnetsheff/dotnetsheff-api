using dotnetsheff.Api.GetAvailableFeedbackEvents;
using Xunit;
using FluentAssertions;

namespace dotnetsheff.Api.Tests
{
    public class TalkParserTests
    {
        [Fact]
        public void ShouldReturnCorrectTalksForTwoSpeakers()
        {
            var input = new Event
            {
                Name = "Chocolatey with Gary Park and HTTP API patterns with Toby Henderson",
                Description = "This event will be split into two parts, Gary Ewan Park presenting Adding a layer of Chocolate(y) and the second half will be Toby Henderson presenting HTTP API patterns."
            };
            var talks = new TalkParser().Parse(input);

            talks.ShouldBeEquivalentTo(new []{
              new {Title = "Adding a layer of Chocolate(y)", Speaker = "Gary Ewan Park"},
              new {Title = "HTTP API patterns", Speaker = "Toby Henderson"},
            });
        }
    }
}




