using Autofac;
using dotnetsheff.Api.AlexaSkill;
using dotnetsheff.Api.GetLatestEvent;
using dotnetsheff.Api.Meetup;

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
        }

        public TService Resolve<TService>()
        {
            return _container.Resolve<TService>();
        }
    }
}