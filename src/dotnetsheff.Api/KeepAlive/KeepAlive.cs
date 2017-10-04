using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace dotnetsheff.Api.KeepAlive
{
    public static class KeepAlive
    {
        [FunctionName("KeepAlive")]
        public static void Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, TraceWriter log)
        {
            // Do nothing...
        }
    }
}
