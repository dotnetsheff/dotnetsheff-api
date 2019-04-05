using System;
using AutoMapper;

namespace dotnetsheff.Api.PostFeedbackEvent
{
    public class EventFeedbackProfile : Profile
    {
        public EventFeedbackProfile()
        {
            CreateMap<EventFeedback, EventTableEntity>()
                .ForMember(x => x.PartitionKey, o => o.MapFrom(x => "Feedback"))
                .ForMember(x => x.RowKey, o => o.MapFrom(x => x.Id))
                .ForMember(x => x.ETag, o => o.MapFrom(x => Guid.NewGuid().ToString()))
                .ForMember(x => x.Timestamp, o => o.MapFrom(x => DateTimeOffset.UtcNow));
            
            CreateMap<TalkFeedback, TalkTableEntity>()
                .ForMember(x => x.PartitionKey, o => o.MapFrom(x => "Feedback"))
                .ForMember(x => x.RowKey, o => o.MapFrom(x => x.Id))
                .ForMember(x => x.ETag, o => o.MapFrom(x => Guid.NewGuid().ToString()))
                .ForMember(x => x.Timestamp, o => o.MapFrom(x => DateTimeOffset.UtcNow));
        }
    }
}
