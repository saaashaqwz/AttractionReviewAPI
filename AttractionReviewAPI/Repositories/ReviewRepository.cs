using Microsoft.EntityFrameworkCore;

namespace AttractionReviewAPI.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly APIDBContext _context;

    public ReviewRepository(APIDBContext context)
    {
        _context = context;
    }

    public IEnumerable<Review> GetAll() => _context.Reviews.ToList();

    public Review GetById(int id)
    {
        var review = _context.Reviews.FirstOrDefault(r => r.Id == id);
        return review;
    }

    public Review Create(Review entity)
    {
        _context.Reviews.Add(entity);
        _context.SaveChanges();
        return entity;
    }

    public Review Update(Review entity)
    {
        _context.Reviews.Update(entity);
        _context.SaveChanges();
        return entity;
    }

    public bool Delete(int id)
    {
        var review = GetById(id);
        if (review != null)
        {
            _context.Reviews.Remove(review);
            _context.SaveChanges();
            return true;
        }
        return false;
    }

    public bool Exists(int id) => _context.Reviews.Any(r => r.Id == id);

    public IEnumerable<Review> GetReviewByAttraction(int attractionId)
    {
        return _context.Reviews
            .Include(r => r.Attraction)
            .Where(r =>  r.AttractionId == attractionId)
            .ToList();
    }
}