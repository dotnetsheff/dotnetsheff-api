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
            var input = new PastEvent
            {
                Name = "Chocolatey with Gary Park and HTTP API patterns with Toby Henderson",
                Description = "This event will be split into two parts, Gary Ewan Park presenting Adding a layer of Chocolate(y) and the second half will be Toby Henderson presenting HTTP API patterns."
            };
            var talks = new TwoSpeakersTalkParser().Parse(input);

            talks.ShouldBeEquivalentTo(new []{
              new {Title = "Adding a layer of Chocolate(y)", Speaker = "Gary Ewan Park"},
              new {Title = "HTTP API patterns", Speaker = "Toby Henderson"},
            });
        }
        [Fact]
        public void ShouldReturnCorrectTalksForOneSpeaker()
        {
            var input = new PastEvent
            {
                Name = "How to parse a file & Kotlin for the curious with Matt Ellis",
                Description = "This event will be broken down into 2 talks, How to parse a file and Kotlin for the curious both being presented by Matt Ellis."
            };
            var talks = new OneSpeakerTwoTalksParser().Parse(input);

            talks.ShouldBeEquivalentTo(new []{
              new {Title = "How to parse a file", Speaker = "Matt Ellis"},
              new {Title = "Kotlin for the curious", Speaker = "Matt Ellis"},
            });
        }
    }
}




