﻿using Alexa.NET.Request;
using Alexa.NET.Response;
using Alexa.NET.Response.Directive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alexa.NET
{
    public class ResponseBuilder
    {
        #region Tell Responses
        public static SkillResponse Tell(IOutputSpeech speechResponse)
        {
            return BuildResponse(speechResponse, true, null, null, null);
        }
        public static SkillResponse TellWithReprompt(IOutputSpeech speechResponse, Reprompt reprompt)
        {
            return BuildResponse(speechResponse, true, null, reprompt, null);
        }

        public static SkillResponse Tell(IOutputSpeech speechResponse, Session sessionAttributes)
        {
            return BuildResponse(speechResponse, true, sessionAttributes, null, null);
        }

        public static SkillResponse TellWithReprompt(IOutputSpeech speechResponse, Reprompt reprompt, Session sessionAttributes)
        {
            return BuildResponse(speechResponse, true, sessionAttributes, reprompt, null);
        }

        public static SkillResponse TellWithCard(IOutputSpeech speechResponse, string title, string content)
        {
            SimpleCard card = new SimpleCard();
            card.Content = content;
            card.Title = title;

            return BuildResponse(speechResponse, true, null, null, card);
        }

        public static SkillResponse TellWithCard(IOutputSpeech speechResponse, string title, string content, Session sessionAttributes)
        {
            SimpleCard card = new SimpleCard();
            card.Content = content;
            card.Title = title;

            return BuildResponse(speechResponse, true, sessionAttributes, null, card);
        }

        public static SkillResponse TellWithLinkAccountCard(IOutputSpeech speechResponse)
        {
            LinkAccountCard card = new LinkAccountCard();

            return BuildResponse(speechResponse, true, null, null, card);
        }

        public static SkillResponse TellWithLinkAccountCard(IOutputSpeech speechResponse, Session sessionAttributes)
        {
            LinkAccountCard card = new LinkAccountCard();

            return BuildResponse(speechResponse, true, sessionAttributes, null, card);
        }

        public static SkillResponse TellWithAskForPermissionsConsentCard(IOutputSpeech speechResponse, IEnumerable<string> permissions)
        {
            AskForPermissionsConsentCard card = new AskForPermissionsConsentCard();
            card.Permissions = permissions.ToList();
            return BuildResponse(speechResponse, true, null, null, card);
        }

        public static SkillResponse TellWithAskForPermissionsConsentCard(IOutputSpeech speechResponse, IEnumerable<string> permissions, Session sessionAttributes)
        {
            AskForPermissionsConsentCard card = new AskForPermissionsConsentCard();
            card.Permissions = permissions.ToList();
            return BuildResponse(speechResponse, true, sessionAttributes, null, card);
        }

        #endregion

        #region Ask Responses
        public static SkillResponse Ask(IOutputSpeech speechResponse, Reprompt reprompt)
        {
            return BuildResponse(speechResponse, false, null, reprompt, null);
        }

        public static SkillResponse Ask(IOutputSpeech speechResponse, Reprompt reprompt, Session sessionAttributes)
        {
            return BuildResponse(speechResponse, false, sessionAttributes, reprompt, null);
        }

        public static SkillResponse AskWithCard(IOutputSpeech speechResponse, string title, string content, Reprompt reprompt)
        {
            return AskWithCard(speechResponse, title, content, reprompt, null);
        }

        public static SkillResponse AskWithCard(IOutputSpeech speechResponse, string title, string content, Reprompt reprompt, Session sessionAttributes)
        {
            SimpleCard card = new SimpleCard();
            card.Content = content;
            card.Title = title;

            return BuildResponse(speechResponse, false, sessionAttributes, reprompt, card);
        }
        #endregion

        #region AudioPlayer Response
        public static SkillResponse AudioPlayerPlay(PlayBehavior playBehavior, string url, string token)
        {
            return AudioPlayerPlay(playBehavior, url, token, 0);
        }

        public static SkillResponse AudioPlayerPlay(PlayBehavior playBehavior, string url, string token, int offsetInMilliseconds)
        {
            return AudioPlayerPlay(playBehavior, url, token, null, offsetInMilliseconds);
        }

        public static SkillResponse AudioPlayerPlay(PlayBehavior playBehavior, string url, string token, string expectedPreviousToken, int offsetInMilliseconds)
        {
            var response = BuildResponse(null, true, null, null, null);
            response.Response.Directives.Add(new AudioPlayerPlayDirective()
            {
                PlayBehavior = playBehavior,
                AudioItem = new AudioItem()
                {
                    Stream = new AudioItemStream()
                    {
                        Url = url,
                        Token = token,
                        ExpectedPreviousToken = expectedPreviousToken,
                        OffsetInMilliseconds = offsetInMilliseconds
                    }
                }
            });

            return response;
        }

        public static SkillResponse AudioPlayerStop()
        {
            var response = BuildResponse(null, true, null, null, null);
            response.Response.Directives.Add(new StopDirective());
            return response;
        }

        public static SkillResponse AudioPlayerClearQueue(ClearBehavior clearBehavior)
        {
            var response = BuildResponse(null, true, null, null, null);
            response.Response.Directives.Add(new ClearQueueDirective()
            {
                ClearBehavior = clearBehavior
            });
            return response;
        }
        #endregion

        #region Dialog Response

        public static SkillResponse DialogDelegate(Intent updatedIntent = null)
        {
            return DialogDelegate(null, updatedIntent);
        }

        public static SkillResponse DialogDelegate(Session attributes, Intent updatedIntent = null)
        {
            var response = BuildResponse(null, false, attributes, null, null);
            response.Response.Directives.Add(new DialogDelegate { UpdatedIntent = updatedIntent });
            return response;
        }

        public static SkillResponse DialogElicitSlot(IOutputSpeech outputSpeech, string slotName, Intent updatedIntent = null)
        {
            return DialogElicitSlot(outputSpeech, slotName, null, updatedIntent);
        }

        public static SkillResponse DialogElicitSlot(IOutputSpeech outputSpeech, string slotName, Session attributes, Intent updatedIntent = null)
        {
            var response = BuildResponse(outputSpeech, false, attributes, null, null);
            response.Response.Directives.Add(new DialogElicitSlot(slotName) { UpdatedIntent = updatedIntent });
            return response;
        }

        public static SkillResponse DialogConfirmSlot(IOutputSpeech outputSpeech, string slotName, 
            Intent updatedIntent = null)
        {
            return DialogConfirmSlot(outputSpeech, slotName, null, updatedIntent);
        }

        public static SkillResponse DialogConfirmSlot(IOutputSpeech outputSpeech, string slotName, Session attributes, Intent updatedIntent = null)
        {
            var response = BuildResponse(outputSpeech, false, attributes, null, null);
            response.Response.Directives.Add(new DialogConfirmSlot(slotName) { UpdatedIntent = updatedIntent });
            return response;
        }

        public static SkillResponse DialogConfirmIntent(IOutputSpeech outputSpeech, Intent updatedIntent = null)
        {
            return DialogConfirmIntent(outputSpeech, null, updatedIntent);
        }

        public static SkillResponse DialogConfirmIntent(IOutputSpeech outputSpeech, Session attributes, Intent updatedIntent = null)
        {
            var response = BuildResponse(outputSpeech, false, attributes, null, null);
            response.Response.Directives.Add(new DialogConfirmIntent { UpdatedIntent = updatedIntent });
            return response;
        }

        #endregion

        public static SkillResponse Empty()
        {
            return BuildResponse(null, true, null, null, null);
        }

        #region Main Response Builder
        private static SkillResponse BuildResponse(IOutputSpeech outputSpeech, bool shouldEndSession, Session sessionAttributes, Reprompt reprompt, ICard card)
        {
            SkillResponse response = new Response.SkillResponse();
            response.Version = "1.0";
            if (sessionAttributes != null) response.SessionAttributes = sessionAttributes.Attributes;

            ResponseBody body = new Response.ResponseBody();
            body.ShouldEndSession = shouldEndSession;
            body.OutputSpeech = outputSpeech;

            if (reprompt != null) body.Reprompt = reprompt;
            if (card != null) body.Card = card;

            response.Response = body;

            return response;
        }
        #endregion
    }
}
