using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace dotnetsheff.Api.AlexaSkill
{
    public static class AlexaSkill
    {
        [FunctionName("AlexaSkill")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "alexa-skill")]HttpRequestMessage req, TraceWriter log)
        {
            var speechlet = Container.Instance.Resolve<EventSpeechlet>(log);

            var res = await speechlet.GetResponseAsync(req)
                .ConfigureAwait(false);

            return res;
        }
    }
}
