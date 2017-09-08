namespace dotnetsheff.Api.GetLatestEvent
{
    public interface IEventToNextEventConvertor
    {
        NextEvent Convert(Event @event);
    }
}