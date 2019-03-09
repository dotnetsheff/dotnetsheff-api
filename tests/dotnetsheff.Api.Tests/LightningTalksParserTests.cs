using System.Collections.Generic;
using System.IO;
using System.Linq;
using dotnetsheff.Api.GetAvailableFeedbackEvents;
using FluentAssertions;
using Xunit;

namespace dotnetsheff.Api.Tests
{
    public class LightningTalksParserTests
    {
        [Theory]
        [MemberData(nameof(LightningTalksCases))]
        public void ShouldReturnCorrectLightningTalks(string description, IEnumerable<Talk> expected)
        {
            var input = new PastEvent
            {
                Description = description
            };

            var talks = new LightningTalksParser().Parse(input).ToArray();

            talks.ShouldBeEquivalentTo(expected);
        }

        public static IEnumerable<object[]> LightningTalksCases =>
            new List<object[]>
            {
                new object[]
                {
                    "not a lightning talk event",
                    Enumerable.Empty<Talk>()
                },
                new object[]
                {
                    "Slot 1",
                    Enumerable.Empty<Talk>()
                },
                new object[]
                {
                    "Slot 1 - FirstName LastName - Test Title Lightning Talk</p>",
                    new []
                    {
                        new Talk
                        {
                            Title = "Test Title Lightning Talk",
                            Speaker = "FirstName LastName"
                        }
                    }
                },
                new object[]
                {
                    @"Slot 1 - Zoltán Lehóczky (@zlehoczky (<a href=""https://twitter.com/@zlehoczky"" class=""linkified"">https://twitter.com/@zlehoczky</a>)) - Hastlayer Talk</p>",
                    new []
                    {
                        new Talk
                        {
                            Title = "Hastlayer Talk",
                            Speaker = "Zoltán Lehóczky"
                        }
                    }
                },
                new object[]
                {
                    File.ReadAllText("lightningtalksrdescription.txt"),
                    new []
                    {
                        new Talk
                        {
                            Title = "Hastlayer Talk",
                            Speaker = "Zoltán Lehóczky"
                        },
                        new Talk
                        {
                            Title = "Dapper",
                            Speaker = "Michael Steele"
                        },
                        new Talk
                        {
                            Title = "Captains Log",
                            Speaker = "Andrew Gunn"
                        },
                        new Talk
                        {
                            Title = "Error Monitoring with Raygun",
                            Speaker = "Kevin Smith"
                        },
                        new Talk
                        {
                            Title = "Ah, push it, push it real good",
                            Speaker = "Elliot Chaim"
                        },
                        new Talk
                        {
                            Title = "Nuance of developing Unity3D apps",
                            Speaker = "Jonathan Eyre"
                        },
                    }
                }
            };
    }
}