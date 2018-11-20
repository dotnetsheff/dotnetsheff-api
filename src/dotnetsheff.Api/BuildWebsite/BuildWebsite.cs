using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace dotnetsheff.Api.BuildWebsite
{
    public static class BuildWebsite
    {
        private static readonly HttpClient HttpClient = new HttpClient()
        {
            BaseAddress = new Uri("https://ci.appveyor.com/"),
            DefaultRequestHeaders = { Authorization = new AuthenticationHeaderValue("Bearer", Environment.GetEnvironmentVariable("AppVeyorApiKey")) }
        };

        [FunctionName("BuildWebsite")]
        public static async Task Run([TimerTrigger("0 5 * * *")]TimerInfo myTimer, TraceWriter log)
        {
            var value = new
            {
                accountName = "kevbite",
                projectSlug = "dotnetsheff",
                branch = "master"
            };

            var responseMessage = await HttpClient.PostAsJsonAsync("api/builds", value);

            responseMessage.EnsureSuccessStatusCode();

            log.Info($"AppVeyor {value.accountName}/{value.projectSlug} {value.branch} build triggered");
        }
    }
}
