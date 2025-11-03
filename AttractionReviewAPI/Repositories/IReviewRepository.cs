namespace AttractionReviewAPI.Repositories;

public interface IReviewRepository : IRepository<Review>
{
    IEnumerable<Review> GetReviewByAttraction(int attractionId);
}