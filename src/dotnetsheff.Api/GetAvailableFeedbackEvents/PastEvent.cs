using Newtonsoft.Json;

namespace dotnetsheff.Api.GetAvailableFeedbackEvents
{
    public class PastEvent
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}