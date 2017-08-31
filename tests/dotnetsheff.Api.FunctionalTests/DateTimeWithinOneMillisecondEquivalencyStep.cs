using System;
using FluentAssertions;
using FluentAssertions.Equivalency;

namespace dotnetsheff.Api.FunctionalTests
{
    public class DateTimeWithinOneMillisecondEquivalencyStep : IEquivalencyStep
    {
        public bool CanHandle(IEquivalencyValidationContext context, IEquivalencyAssertionOptions config)
        {
            var subjectType = config.GetSubjectType(context);

            return subjectType != null && subjectType == typeof(DateTime);
        }

        public bool Handle(IEquivalencyValidationContext context, IEquivalencyValidator parent, IEquivalencyAssertionOptions config)
        {
            var actual = (DateTime)context.Subject;
            var expected = (DateTime)context.Expectation;

            actual.Should().BeCloseTo(expected, 1);

            return true;
        }
    }
}