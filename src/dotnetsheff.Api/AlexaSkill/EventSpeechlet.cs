using System;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using AlexaSkillsKit.Authentication;
using AlexaSkillsKit.Json;
using AlexaSkillsKit.Speechlet;
using AlexaSkillsKit.UI;
using Microsoft.Azure.WebJobs.Host;
using Refit;

namespace dotnetsheff.Api.AlexaSkill
{
    public class EventSpeechlet : SpeechletAsync
    {
        private readonly TraceWriter _log;
        private readonly IAlexaSkillEventQuery _alexaSkillEventQuery;
        private readonly INextEventSsmlGenerator _ssmlGenerator;

        public EventSpeechlet(TraceWriter log, IAlexaSkillEventQuery alexaSkillEventQuery, INextEventSsmlGenerator ssmlGenerator)
        {
            _log = log;
            _alexaSkillEventQuery = alexaSkillEventQuery;
            _ssmlGenerator = ssmlGenerator;
        }

        public override async Task<SpeechletResponse> OnIntentAsync(IntentRequest request, Session session)
        {
            _log.Info($"OnIntent requestId={request.RequestId}, sessionId={session.SessionId}");

            if (request.Intent.Name.Equals("GetNextEvent"))
            {
                var @event = await _alexaSkillEventQuery.Execute();

                var speech = new SsmlOutputSpeech
                {
                    Ssml = _ssmlGenerator.Generate(@event.Name, @event.Time)
                };
                
                var response = new SpeechletResponse
                {
                    ShouldEndSession = true,
                    OutputSpeech = speech
                };

                return response;
            }

            throw new SpeechletException("Invalid Intent");
        }

        public override Task<SpeechletResponse> OnLaunchAsync(LaunchRequest request, Session session)
        {
            _log.Info($"OnLaunch requestId={request.RequestId}, sessionId={session.SessionId}");
            
            var ssml = "<speak>Welcome to the Alexa <sub alias=\"dot net sheff\">dotnetsheff</sub> app, you can ask me for the next event.</speak>";

            var speech = new SsmlOutputSpeech { Ssml = ssml };

            return Task.FromResult(new SpeechletResponse
            {
                ShouldEndSession = true,
                OutputSpeech = speech
            });
        }

        public override Task OnSessionStartedAsync(SessionStartedRequest sessionStartedRequest, Session session)
        {
            return Task.CompletedTask;
        }

        public override Task OnSessionEndedAsync(SessionEndedRequest sessionEndedRequest, Session session)
        {
            return Task.CompletedTask;
        }

        public override bool OnRequestValidation(SpeechletRequestValidationResult result, DateTime referenceTimeUtc,
            SpeechletRequestEnvelope requestEnvelope)
        {
            if (requestEnvelope.Session.Application.Id != Environment.GetEnvironmentVariable("AlexaSkillId"))
            {
                var message = $"Unknown alex skill application. id = '{requestEnvelope.Session.Application.Id}'";
                _log.Warning(message);

                return false;
            }

            return base.OnRequestValidation(result, referenceTimeUtc, requestEnvelope);
        }
    }
}