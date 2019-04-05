using System.Collections.Generic;
using System.IO;
using System.Linq;
using dotnetsheff.Api.GetAvailableFeedbackEvents;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace dotnetsheff.Api.Tests.GetAvailableFeedbackEvents.TalksParsersTests
{
    public class OneSpeakerOneTalksParserTests
    {
        [Theory]
        [MemberData(nameof(OneSpeakerOneTalkCases))]
        public void ShouldReturnCorrectTalkForSpeaker(PastEvent pastEvent, IEnumerable<Talk> expectedTalks) => 
            new OneSpeakerOneTalksParser().Parse(pastEvent).ShouldAllBeEquivalentTo(expectedTalks);

        public static IEnumerable<object[]> OneSpeakerOneTalkCases =>
            new List<object[]>
            {
                new object[]
                {
                    JsonConvert.DeserializeObject<PastEvent[]>(File.ReadAllText("onespeakeronetalkdescription.txt")).First(),
                    new[]
                    {
                        new Talk
                        {
                            Title = "C# 8 New features",
                            Speaker = "Jon Skeet"
                        }
                    }
                }
            };
    }
}
