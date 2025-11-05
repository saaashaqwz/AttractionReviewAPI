using AttractionReviewAPI.DTO;

namespace AttractionReviewAPI.Services;

public interface IReviewService
{
    IEnumerable<ReviewDTO> GetAllReviews();
    ReviewDTO GetByIdReview(int id);
    ReviewDTO CreateReview(CreateReviewDTO createReviewDTO);
    ReviewDTO UpdateReview(int id, UpdateReviewDTO updateReviewDTO);
    bool DeleteReview(int id);
}