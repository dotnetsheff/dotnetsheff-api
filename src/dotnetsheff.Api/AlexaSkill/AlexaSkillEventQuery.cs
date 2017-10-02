using System.Linq;
using System.Threading.Tasks;
using dotnetsheff.Api.Meetup;

namespace dotnetsheff.Api.AlexaSkill
{
    public class AlexaSkillEventQuery : IAlexaSkillEventQuery
    {
        private readonly IMeetupApi _meetupApi;
        private readonly string _group;
        private readonly string _apiKey;

        public AlexaSkillEventQuery(IMeetupApi meetupApi, IMeetupSettings meetupSettings)
        {
            _meetupApi = meetupApi;
            _group = meetupSettings.GroupName;
            _apiKey = meetupSettings.ApiKey;
        }

        public async Task<AlexaSkillEvent> Execute()
        {
            var events = await _meetupApi.GetAlexaSkillEventsAsync(_group, _apiKey, "upcoming", "1", FieldsToOmitJoined);
           
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
            "visibility",
            "yes_rsvp_count",
            "link",
            "description"
        };
    }
}