using System;
using AutoMapper;

namespace dotnetsheff.Api.PostFeedbackEvent
{
    public class EventFeedbackProfile : Profile
    {
        public EventFeedbackProfile()
        {
            CreateMap<EventFeedback, EventFeedbackTableEntity>()
                .ForMember(x => x.PartitionKey, o => o.MapFrom(x => x.Id))
                .ForMember(x => x.RowKey, o => o.MapFrom(x => "Event"))
                .ForMember(x => x.ETag, o => o.Ignore())
                .ForMember(x => x.Timestamp, o => o.Ignore());
            
            CreateMap<TalkFeedback, TalkFeedbackTableEntity>()
                .ForMember(x => x.PartitionKey, o => o.Ignore())
                .ForMember(x => x.RowKey, o => o.MapFrom(x => x.Id))
                .ForMember(x => x.ETag, o => o.Ignore())
                .ForMember(x => x.Timestamp, o => o.Ignore());
        }
    }
}
