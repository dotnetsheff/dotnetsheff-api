using System.Collections.Generic;
using System.Threading.Tasks;

namespace dotnetsheff.Api.GetAvailableFeedbackEvents
{
    public interface ILastEventsQuery
    {
        Task<IEnumerable<PastEvent>> Execute(int eventCount);
    }
}