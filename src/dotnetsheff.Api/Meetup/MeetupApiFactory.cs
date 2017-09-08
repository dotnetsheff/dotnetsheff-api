using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Refit;

namespace dotnetsheff.Api.Meetup
{
    public class MeetupApiFactory : IMeetupApiFactory
    {
        private readonly IMeetupSettings _settings;

        public MeetupApiFactory(IMeetupSettings settings)
        {
            _settings = settings;
        }

        public IMeetupApi Create()
        {;
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy(),
                },
            };
            jsonSerializerSettings.Converters.Insert(0, new MillisecondEpochConverter());
            var api = RestService.For<IMeetupApi>(_settings.BaseUri, new RefitSettings
            {
                JsonSerializerSettings = jsonSerializerSettings
            });

            return api;
        }

        
    }
}