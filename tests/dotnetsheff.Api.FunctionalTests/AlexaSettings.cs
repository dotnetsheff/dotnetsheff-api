using System;

namespace dotnetsheff.Api.FunctionalTests
{
    public static class AlexaSettings
    {
        public static string SkillId { get; } = Guid.NewGuid().ToString();
    }
}