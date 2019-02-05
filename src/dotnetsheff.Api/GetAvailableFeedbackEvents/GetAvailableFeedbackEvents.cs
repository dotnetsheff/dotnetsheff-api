using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace dotnetsheff.Api.GetAvailableFeedbackEvents
{
    public class GetAvailableFeedbackEvents
    {
        [FunctionName("GetAvailableFeedbackEvents")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "feedback/available-events")] HttpRequestMessage req,
            TraceWriter log)
        {
            log.Info("Getting feedback for available event");

            // Fetch last 3 events from API

            // Parse out talks from description
            Container.Instance.Resolve<ITalkParser>(log).Parse(new Event());

            // build some model

            // convert to json 

            // vomit over wire.


            var json = @"[
  {
    ""id"": ""254791420-meetup-id"",
    ""title"": ""Chocolatey with Gary Park and HTTP API patterns with Toby Henderson"",
    ""date"": ""2019-01-28"",
    ""talks"": [
      {
        ""title"": ""Adding a layer of Chocolate(y)"",
        ""speaker"": ""Gary Ewan Park""
      },
      {
        ""title"": ""HTTP API patterns"",
        ""speaker"": ""Toby Henderson""
      }
    ]
  },
  {
    ""id"": ""254791420-meetup-id"",
    ""title"": ""Chocolatey with Gary Park and HTTP API patterns with Toby Henderson"",
    ""date"": ""2018-11-06"",
    ""talks"": [
      {
        ""title"": ""Log Analytics"",
        ""speaker"": ""Steve Spencer""
      },
      {
        ""title"": ""Azure Machine Learning for Developers"",
        ""speaker"": ""Steve Spencer""
      }
    ]
  },
  {
    ""id"": ""254791420-meetup-id"",
    ""title"": ""Lightning Talks!"",
    ""date"": ""2018-07-03"",
    ""talks"": [
      {
        ""title"": ""Hastlayer Talk"",
        ""speaker"": ""Zoltán Lehóczky""
      },
      {
        ""title"": ""Dapper"",
        ""speaker"": ""Michael Steele""
      },
      {
        ""title"": ""Captains Log"",
        ""speaker"": ""Andrew Gunn""
      },
      {
        ""title"": ""Error Monitoring with Raygun"",
        ""speaker"": ""Kevin Smith""
      }
    ]
  }
]";

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
        }
    }
}
