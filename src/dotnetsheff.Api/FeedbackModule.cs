using Autofac;
using dotnetsheff.Api.GetAvailableFeedbackEvents;
using dotnetsheff.Api.Meetup;

namespace dotnetsheff.Api
{
    public class FeedbackModule : Module
    {
        private const string GROUP_NAME = "dotnetsheff";
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterAssemblyTypes(ThisAssembly)
                .Where(x => x.Name.EndsWith("TalksParser"))
                .AsImplementedInterfaces();

            builder
                .Register(ctx => new LastEventsQuery(ctx.Resolve<IMeetupApi>(), GROUP_NAME, ctx.Resolve<IMeetupSettings>().ApiKey))
                .As<ILastEventsQuery>();
        }
    }
}
