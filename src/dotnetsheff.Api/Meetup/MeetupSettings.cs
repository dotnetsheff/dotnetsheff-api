using System;

namespace dotnetsheff.Api.Meetup
{
    public class MeetupSettings : IMeetupSettings
    {
        public string BaseUri => Environment.GetEnvironmentVariable("MeetupApiBaseUri");

        public string ApiKey => Environment.GetEnvironmentVariable("MeetupApiKey");

        public string GroupName { get; } = "dotnetsheff";
    }
}