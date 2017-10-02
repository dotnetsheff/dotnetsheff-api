using System;

namespace dotnetsheff.Api.AlexaSkill
{
    public interface INextEventSsmlGenerator
    {
        string Generate(string eventName, DateTime eventDate);
    }
}