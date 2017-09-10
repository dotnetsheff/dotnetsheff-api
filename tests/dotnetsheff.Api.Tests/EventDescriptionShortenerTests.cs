using System;
using dotnetsheff.Api.GetLatestEvent;
using FluentAssertions;
using Xunit;

namespace dotnetsheff.Api.Tests
{
    
    public class EventDescriptionShortenerTests
    {
        [Fact]
        public void ShouldReturnFirstParagraphTextBody()
        {
            var sut = new EventDescriptionShortener();

            var expected = "first 1";
            var actual = sut.Shorten($"<p>{expected}</p><p>second 2</p>");

            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldStripAllHtmlWithinParagraph()
        {
            var sut = new EventDescriptionShortener();

            var expected = "first 1";
            var actual = sut.Shorten($"<p><span><b>f</b>irst</span> <i>1</i></p>");

            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldReturnFullStringForText()
        {
            var sut = new EventDescriptionShortener();

            var thisIsJustSomeText = "This is just some text";
            var actual = sut.Shorten(thisIsJustSomeText);

            actual.Should().Be(thisIsJustSomeText);
        }
    }
}
