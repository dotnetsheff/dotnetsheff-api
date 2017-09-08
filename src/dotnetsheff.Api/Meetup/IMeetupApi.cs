using System.Collections.Generic;
using System.Threading.Tasks;
using dotnetsheff.Api.AlexaSkill;
using dotnetsheff.Api.GetLatestEvent;
using Refit;

namespace dotnetsheff.Api.Meetup
{
    public interface IMeetupApi
    {
        [Get("/{group}/events")]
        Task<List<Event>> GetEventsAsync(string group, string apiKey, string status, string page, string omit);

        [Get("/{group}/events")]
        Task<List<AlexaSkillEvent>> GetAlexaSkillEventsAsync(string group, string apiKey, string status, string page, string omit);
    }
}