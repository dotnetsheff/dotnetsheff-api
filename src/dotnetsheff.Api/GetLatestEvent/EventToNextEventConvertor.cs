using dotnetsheff.Api.Meetup;

namespace dotnetsheff.Api.GetLatestEvent
{
    public class EventToNextEventConvertor
    {
        private readonly EventDescriptionShortener _eventDescriptionShortener;

        public EventToNextEventConvertor(EventDescriptionShortener eventDescriptionShortener)
        {
            _eventDescriptionShortener = eventDescriptionShortener;
        }

        public NextEvent Convert(Event @event)
        {
            var nextEvent = new NextEvent()
            {
                Id = @event.Id,
                Time = @event.Time,
                Link = @event.Link,
                YesRsvpCount = @event.YesRsvpCount,
                Name = @event.Name,
                ShortDescription = _eventDescriptionShortener.Shorten(@event.Description)
            };

            return nextEvent;
        }
    }
}