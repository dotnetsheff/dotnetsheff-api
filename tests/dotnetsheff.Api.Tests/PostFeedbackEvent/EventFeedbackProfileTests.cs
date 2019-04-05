using AutoFixture;
using AutoMapper;
using dotnetsheff.Api.PostFeedbackEvent;
using FluentAssertions;
using Xunit;

namespace dotnetsheff.Api.Tests.PostFeedbackEvent
{
    public class EventFeedbackProfileTests
    {
        private const string PARTITION_KEY = "Feedback";
        public EventFeedbackProfileTests() => Mapper.Initialize(cfg => cfg.AddProfile<EventFeedbackProfile>());

        [Fact]
        public void ShouldBeConfiguredCorrectly() => Mapper.AssertConfigurationIsValid();

        [Fact]
        public void ShouldMapEventCorrectly()
        {
            var expected = new Fixture().Build<EventFeedback>().Create();

            var actual = Mapper.Map<EventTableEntity>(expected);

            actual.ShouldBeEquivalentTo(expected, options => options.ExcludingMissingMembers());
            actual.PartitionKey.Should().Be(PARTITION_KEY);
            actual.RowKey.Should().Be(expected.Id);
        }
    }
}
