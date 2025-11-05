using AttractionReviewAPI.DTO;
using AutoMapper;

namespace AttractionReviewAPI.Profiles;

public class ReviewProfile : Profile
{
    public ReviewProfile() 
    {
        CreateMap<Review, ReviewDTO>();
    }
}