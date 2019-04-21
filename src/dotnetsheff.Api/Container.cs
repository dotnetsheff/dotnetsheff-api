using Autofac;
using AutoMapper;
using dotnetsheff.Api.AlexaSkill;
using dotnetsheff.Api.GetLatestEvent;
using dotnetsheff.Api.Meetup;
using dotnetsheff.Api.PostFeedbackEvent;
using Microsoft.Azure.WebJobs.Host;

namespace dotnetsheff.Api
{
    public class Container
    {
        private const string GROUP_NAME = "dotnetsheff";

        private readonly IContainer _container;

        public static Container Instance { get; } = new Container();

        public Container()
        {
            ContainerBuilder builder = new ContainerBuilder();

            BuildMeetupApi(builder);

            BuildGetLatestEventTypes(builder);

            BuildAutomapper(builder);

            builder.RegisterModule<FeedbackModule>();

            builder.RegisterType<AlexaSkillEventQuery>().As<IAlexaSkillEventQuery>();

            builder.RegisterType<EventSpeechlet>().AsSelf();

            _container = builder.Build();
        }

        private void BuildAutomapper(ContainerBuilder builder)
        {
            builder.Register(ctx =>
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<EventFeedbackProfile>();
                });

                return new Mapper(config, ctx.Resolve<ILifetimeScope>().Resolve);
            }).As<IMapper>().SingleInstance();
        }

        private static void BuildMeetupApi(ContainerBuilder builder)
        {
            builder.RegisterType<MeetupSettings>().As<IMeetupSettings>();

            builder.RegisterType<MeetupApiFactory>().As<IMeetupApiFactory>();

            builder.Register(ctx => ctx.Resolve<IMeetupApiFactory>().Create())
                .As<IMeetupApi>();
        }

        private static void BuildGetLatestEventTypes(ContainerBuilder builder)
        {
            builder.Register(ctx => new NextEventQuery(ctx.Resolve<IMeetupApi>(), GROUP_NAME, ctx.Resolve<IMeetupSettings>().ApiKey))
                .As<INextEventQuery>();

            builder.RegisterType<EventToNextEventConvertor>().As<IEventToNextEventConvertor>();

            builder.RegisterType<EventDescriptionShortener>().As<IEventDescriptionShortener>();

            builder.RegisterType<NextEventSsmlGenerator>().As<INextEventSsmlGenerator>();

            builder.RegisterType<AlexaSkillSettings>().As<IAlexaSkillSettings>();
        }

        public TService Resolve<TService>(TraceWriter log)
        {
            return _container.Resolve<TService>(TypedParameter.From(log));
        }
    }
}