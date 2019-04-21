using System;
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
            actual.RowKey.Should().NotBe(Guid.Empty.ToString());
        }


        [Fact]
        public void ShouldMapTalksCorrectly()
        {
            var expected = new Fixture().Build<EventFeedback>().Create();

            var actual = _mapper.Map<TalkFeedbackTableEntity[]>(expected.Talks, ops => ops.Items.Add("EventId", expected.Id));

            actual.ShouldBeEquivalentTo(expected.Talks, options => options.ExcludingMissingMembers());
            actual.Select(x => x.PartitionKey)
                .ShouldBeEquivalentTo(expected.Talks.Select(x => $"{expected.Id}-{x.Id}").ToArray());

            actual.Select(x => x.RowKey).Should().NotContain(Guid.Empty.ToString());
        }
    }
}
