using System.Threading.Tasks;

namespace dotnetsheff.Api.AlexaSkill
{
    public interface IAlexaSkillEventQuery
    {
        Task<AlexaSkillEvent> Execute();
    }
}