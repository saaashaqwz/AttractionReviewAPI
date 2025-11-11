using AttractionReviewAPI.DTO;
using AutoMapper;

namespace AttractionReviewAPI.Profiles;

public class ReviewProfile : Profile
{
    public ReviewProfile()
    {
        CreateMap<UpdateReviewDTO, Review>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.AttractionId, opt => opt.Ignore())
            .ForMember(dest => dest.Attraction, opt => opt.Ignore());
        
        CreateMap<Attraction, AttractionDTO>();
        CreateMap<CreateAttractionDTO, Attraction>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Reviews, opt => opt.Ignore());
        
        CreateMap<CreateReviewDTO, Review>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.AttractionId, opt => opt.Ignore())
            .ForMember(dest => dest.Attraction, opt => opt.Ignore());

        CreateMap<UpdateReviewDTO, Review>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.AttractionId, opt => opt.Ignore())
            .ForMember(dest => dest.Attraction, opt => opt.Ignore());
    }
}