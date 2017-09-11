using System;

namespace dotnetsheff.Api.AlexaSkill
{
    public class AlexaSkillSettings : IAlexaSkillSettings
    {
        public AlexaSkillSettings()
        {
            if (bool.TryParse(Environment.GetEnvironmentVariable("AcceptInvalidAlexaSignature"), out bool result))
            {
                AcceptInvalidAlexaSignature = result;
            }
        }

        public bool AcceptInvalidAlexaSignature { get; }

        public string ApplicationId { get; } = Environment.GetEnvironmentVariable("AlexaSkillId");
    }
}