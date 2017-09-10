using Autofac;
using Autofac.Core;
using dotnetsheff.Api.AlexaSkill;
using dotnetsheff.Api.GetLatestEvent;
using dotnetsheff.Api.Meetup;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Build.Framework;

namespace dotnetsheff.Api
{
    public class Container
    {
        private readonly IContainer _container;

        public static Container Instance { get; } = new Container();

        public Container()
        {
            ContainerBuilder builder = new ContainerBuilder();

            BuildGetLatestEventTypes(builder);

            builder.RegisterType<AlexaSkillEventQuery>().As<IAlexaSkillEventQuery>();

            builder.RegisterType<EventSpeechlet>().AsSelf();

            _container = builder.Build();
        }

        private static void BuildGetLatestEventTypes(ContainerBuilder builder)
        {
            builder.RegisterType<MeetupSettings>().As<IMeetupSettings>();

            builder.RegisterType<MeetupApiFactory>().As<IMeetupApiFactory>();

            builder.Register(ctx => ctx.Resolve<IMeetupApiFactory>().Create())
                .As<IMeetupApi>();

            builder.Register(ctx => new NextEventQuery(ctx.Resolve<IMeetupApi>(), "dotnetsheff", ctx.Resolve<IMeetupSettings>().ApiKey))
                .As<INextEventQuery>();

            builder.RegisterType<EventToNextEventConvertor>().As<IEventToNextEventConvertor>();

            builder.RegisterType<EventDescriptionShortener>().As<IEventDescriptionShortener>();

            builder.RegisterType<NextEventSsmlGenerator>().As<INextEventSsmlGenerator>();
        }

        public TService Resolve<TService>(TraceWriter log)
        {
            return _container.Resolve<TService>(TypedParameter.From(log));
        }
    }
}