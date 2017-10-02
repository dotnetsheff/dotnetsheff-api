using System;
using System.Xml.Linq;

namespace dotnetsheff.Api.AlexaSkill
{
    public class NextEventSsmlGenerator : INextEventSsmlGenerator
    {
        public string Generate(string eventName, DateTime eventDate)
        {
            var name = new XText(eventName);
            var ssml = $"<speak>The next <sub alias=\"dot net sheff\">dotnetsheff</sub> event is {name} on <say-as interpret-as=\"date\">{eventDate:yyyyMMdd}</say-as>.</speak>";

            return ssml;
        }
    }
}