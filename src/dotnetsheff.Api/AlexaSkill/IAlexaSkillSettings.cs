namespace dotnetsheff.Api.AlexaSkill
{
    public interface IAlexaSkillSettings
    {
        bool AcceptInvalidAlexaSignature { get; }

        string ApplicationId { get; }
    }
}