using dotnetsheff.Api.Meetup;

namespace dotnetsheff.Api.GetLatestEvent
{
    public static class QueryFactory
    {
        public static NextEventQuery CreateNextEventQuery()
        {
            var meetupSettings = new MeetupSettings();
            var api = new MeetupApiFactory(new MeetupSettings()).Create();
            var nextEventQuery = new NextEventQuery(api, "dotnetsheff", meetupSettings.ApiKey);

            return nextEventQuery;
        }
    }
}