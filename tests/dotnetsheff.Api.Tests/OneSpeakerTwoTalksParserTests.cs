using dotnetsheff.Api.GetAvailableFeedbackEvents;
using FluentAssertions;
using Xunit;

namespace dotnetsheff.Api.Tests
{
    public class OneSpeakerTwoTalksParserTests
    {
        [Fact]
        public void ShouldReturnCorrectTalksForOneSpeaker()
        {
            var input = new PastEvent
            {
                Name = "How to parse a file & Kotlin for the curious with Matt Ellis",
                Description = "This event will be broken down into 2 talks, How to parse a file and Kotlin for the curious both being presented by Matt Ellis."
            };
            var talks = new OneSpeakerTwoTalksParser().Parse(input);

            talks.ShouldBeEquivalentTo(new[]{
                new {Title = "How to parse a file", Speaker = "Matt Ellis"},
                new {Title = "Kotlin for the curious", Speaker = "Matt Ellis"},
            });
        }


    }
}