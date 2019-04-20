using AutoFixture;
using AutoMapper;
using dotnetsheff.Api.PostFeedbackEvent;
using FluentAssertions;
using Xunit;

namespace dotnetsheff.Api.Tests.PostFeedbackEvent
{
    public class EventFeedbackProfileTests
    {
        private readonly IMapper _mapper;
        private const string ROW_KEY = "Event";
        public EventFeedbackProfileTests() => _mapper = new MapperConfiguration(cfg => cfg.AddProfile<EventFeedbackProfile>()).CreateMapper();

        [Fact]
        public void ShouldBeConfiguredCorrectly() => _mapper.ConfigurationProvider.AssertConfigurationIsValid();

        [Fact]
        public void ShouldMapEventCorrectly()
        {
            var expected = new Fixture().Build<EventFeedback>().Create();

            var actual = _mapper.Map<EventFeedbackTableEntity>(expected);

            actual.ShouldBeEquivalentTo(expected, options => options.ExcludingMissingMembers());
            actual.PartitionKey.Should().Be(expected.Id);
            actual.RowKey.Should().Be(ROW_KEY);
        }
    }
}
