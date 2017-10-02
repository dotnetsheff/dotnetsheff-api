using System.Linq;
using HtmlAgilityPack;

namespace dotnetsheff.Api.GetLatestEvent
{
    public class EventDescriptionShortener : IEventDescriptionShortener
    {
        public string Shorten(string description)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(description);

            var pTag = doc.DocumentNode.Descendants("p")
                .FirstOrDefault();

            return pTag?.InnerText ?? description;
        }
    }
}