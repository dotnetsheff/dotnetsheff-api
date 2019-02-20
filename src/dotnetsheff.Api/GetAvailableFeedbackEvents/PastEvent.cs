using Newtonsoft.Json;

namespace dotnetsheff.Api.GetAvailableFeedbackEvents
{
    public class PastEvent
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}