using System.Linq;
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
        public EventFeedbackProfileTests()
        {
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile<EventFeedbackProfile>());
            _mapper = configuration.CreateMapper();
        }

        [Fact]
        public void ShouldBeConfiguredCorrectly()
            => _mapper.ConfigurationProvider.AssertConfigurationIsValid();

        [Fact]
        public void ShouldMapEventCorrectly()
        {
            var expected = new Fixture().Build<EventFeedback>().Create();

            var actual = _mapper.Map<EventFeedbackTableEntity>(expected);

            actual.ShouldBeEquivalentTo(expected, options => options.ExcludingMissingMembers());
            actual.PartitionKey.Should().Be(expected.Id);
            actual.RowKey.Should().Be(ROW_KEY);
        }


        [Fact]
        public void ShouldMapTalksCorrectly()
        {
            var expected = new Fixture().Build<EventFeedback>().Create();

            var actual = _mapper.Map<TalkFeedbackTableEntity[]>(expected.Talks, ops => ops.Items.Add("EventId", expected.Id));

            actual.ShouldBeEquivalentTo(expected.Talks, options => options.ExcludingMissingMembers());
            actual.Select(x => x.PartitionKey).Distinct().ShouldBeEquivalentTo(new []{expected.Id});
            actual.Select(x => x.RowKey).ShouldBeEquivalentTo(expected.Talks.Select(x => x.Id));
        }
    }
}
