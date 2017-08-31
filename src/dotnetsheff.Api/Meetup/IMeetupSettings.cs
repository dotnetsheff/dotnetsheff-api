namespace dotnetsheff.Api.Meetup
{
    public interface IMeetupSettings
    {
        string BaseUri { get; }

        string ApiKey { get; }
    }
}