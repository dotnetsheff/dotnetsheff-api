using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace dotnetsheff.Api.GetLatestEvent
{
    public class NextEvent
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("time")]
        public DateTime Time { get; set; }

        [JsonProperty("yesRsvpCount")]
        public int YesRsvpCount { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("shortDescription")]
        public string ShortDescription { get; set; }
    }
}
