using AttractionReviewAPI.DTO;
using AutoMapper;

namespace AttractionReviewAPI.Profiles;

public class AttractionProfile : Profile
{
    public AttractionProfile()
    {
        CreateMap<Attraction, AttractionDTO>();

        CreateMap<CreateAttractionDTO, Attraction>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Reviews, opt => opt.Ignore());
    }
}