using System;

namespace dotnetsheff.Api.FunctionalTests
{
    public static class MeetupSettings
    {
        public static string MeetupApiBaseUri { get; } = $"http://localhost:{new Random().Next(49152, 65535)}";

        public static string MeetupApiKey { get; } = Guid.NewGuid().ToString();
    }
}