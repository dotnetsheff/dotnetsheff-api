using System.Collections.Generic;
using System.Threading.Tasks;
using dotnetsheff.Api.Meetup;

namespace dotnetsheff.Api.GetAvailableFeedbackEvents
{
    public class LastEventsQuery : ILastEventsQuery
    {
        private readonly IMeetupApi _meetupApi;
        private readonly string _group;
        private readonly string _apiKey;

        public LastEventsQuery(IMeetupApi meetupApi, string group, string apiKey)
        {
            _meetupApi = meetupApi;
            _group = group;
            _apiKey = apiKey;
        }
        public async Task<IEnumerable<PastEvent>> Execute(int eventCount) => 
            await _meetupApi.GetPastEventsAsync(_group, _apiKey, "past", eventCount.ToString(), "id,name,description", "true");
    }
}