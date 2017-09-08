using System.Linq;
using System.Threading.Tasks;
using dotnetsheff.Api.Meetup;

namespace dotnetsheff.Api.GetLatestEvent
{
    public class NextEventQuery : INextEventQuery
    {
        private readonly IMeetupApi _meetupApi;
        private readonly string _group;
        private readonly string _apiKey;

        public NextEventQuery(IMeetupApi meetupApi, string group, string apiKey)
        {
            _meetupApi = meetupApi;
            _group = group;
            _apiKey = apiKey;
        }

        public async Task<Event> Execute()
        {
            var events = await _meetupApi.GetEventsAsync(_group, _apiKey, "upcoming", "1", FieldsToOmitJoined);
           
            return events.FirstOrDefault();
        }

        private static string FieldsToOmitJoined { get; } = string.Join(",", GetFieldsToOmit());

        private static string[] GetFieldsToOmit() => new[]
        {
            "created",
            "status",
            "updated",
            "utc_offset",
            "waitlist_count",
            "venue",
            "group",
            "manual_attendance_count",
            "visibility"
        };
    }
}