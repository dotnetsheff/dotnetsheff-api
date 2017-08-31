using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;

namespace dotnetsheff.Api.Meetup
{
    public interface IMeetupApi
    {
        [Get("/{group}/events")]
        Task<List<Event>> GetEventsAsync(string group, string apiKey, string status, string page, string omit);
    }
}