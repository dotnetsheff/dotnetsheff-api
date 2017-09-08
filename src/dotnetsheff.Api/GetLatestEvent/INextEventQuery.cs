using System.Threading.Tasks;

namespace dotnetsheff.Api.GetLatestEvent
{
    public interface INextEventQuery
    {
        Task<Event> Execute();
    }
}