using Microsoft.EntityFrameworkCore;

namespace AttractionReviewAPI.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly APIDBContext _context;

    public ReviewRepository(APIDBContext context)
    {
        _context = context;
    }

    //<summary>
    // получает все отзывы из базы данных
    //</summary>
    public IEnumerable<Review> GetAll() => _context.Reviews.ToList();

    // <summary>
    // получает отзыв по указанному идентификатору
    // </summary>
    // <param name="id">идентификатор отзыва</param>
    public Review GetById(int id)
    {
        var review = _context.Reviews.FirstOrDefault(r => r.Id == id);
        return review;
    }

    // <summary>
    // создает новый отзыв в базе данных
    // </summary>
    // <param name="entity">объект отзыва для создания</param>
    public Review Create(Review entity)
    {
        _context.Reviews.Add(entity);
        _context.SaveChanges();
        return entity;
    }

    // <summary>
    // обновляет существующий отзыв в базе данных
    // </summary>
    // <param name="entity">объект отзыва с обновленными данными</param>
    public Review Update(Review entity)
    {
        _context.Reviews.Update(entity);
        _context.SaveChanges();
        return entity;
    }

    // <summary>
    // удаляет отзыв по указанному идентификатору
    // </summary>
    // <param name="id">идентификатор отзыва для удаления</param>
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

    // <summary>
    // проверяет существование отзыва по указанному идентификатору
    // </summary>
    // <param name="id">идентификатор отзыва</param>
    public bool Exists(int id) => _context.Reviews.Any(r => r.Id == id);

    
    // <summary>
    // получает все отзывы для указанной достопримечательности
    // </summary>
    // <param name="attractionId">идентификатор достопримечательности</param>
    public IEnumerable<Review> GetReviewByAttraction(int attractionId)
    {
        return _context.Reviews
            .Include(r => r.Attraction)
            .Where(r =>  r.AttractionId == attractionId)
            .ToList();
    }
}