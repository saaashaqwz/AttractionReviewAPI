using AttractionReviewAPI.DTO;
using AttractionReviewAPI.Repositories;

namespace AttractionReviewAPI.Services;

public class ReviewService : IReviewService
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IAttractionRepository _attractionRepository;

    public ReviewService(IReviewRepository reviewRepository, IAttractionRepository attractionRepository)
    {
        _reviewRepository = reviewRepository;
        _attractionRepository = attractionRepository;
    }
    
    // <summary>
    // преобразует объект Review в ReviewDTO
    // </summary>
    // <param name="review">объект отзыва из базы данных</param>
    private static ReviewDTO MapToReviewDTO(Review review)
    {
        return new ReviewDTO
        {
            Id = review.Id,
            Title = review.Title,
            Content = review.Content,
            Rating = review.Rating,
            Author = review.Author,
            CreatedDate = review.CreatedDate,
            AttractionId = review.AttractionId
        };
    }
    
    // <summary>
    // получает все отзывы
    // </summary>
    public IEnumerable<ReviewDTO> GetAllReviews()
    {
        var reviews = _reviewRepository.GetAll();
        return reviews.Select(MapToReviewDTO);
    }

    // <summary>
    // получает отзыв по идентификатору
    // </summary>
    // <param name="id">идентификатор отзыва</param>
    public ReviewDTO GetByIdReview(int id)
    {
        var review = _reviewRepository.GetById(id);
        return review == null ? null : MapToReviewDTO(review);
    }

    // <summary>
    // создает новый отзыв
    // </summary>
    // <param name="createReviewDTO">DTO с данными для создания отзыва</param>
    public ReviewDTO CreateReview(CreateReviewDTO createReviewDTO)
    {
        int attractionID;
        if (createReviewDTO.AttractionId.HasValue)
        {
            var existingAttraction = _attractionRepository.GetById(createReviewDTO.AttractionId.Value);
            if (existingAttraction == null)
                throw new ArgumentException("Достопримечательность не найден");
            attractionID = existingAttraction.Id;
        }
        else if (createReviewDTO.newAttraction != null)
        {
            var newAttraction = new Attraction
            {
                Name = createReviewDTO.newAttraction.Name,
                Description = createReviewDTO.newAttraction.Description,
                Location = createReviewDTO.newAttraction.Location,
                Category = createReviewDTO.newAttraction.Category
            };

            var createdAuthor = _attractionRepository.Create(newAttraction);
            attractionID = createdAuthor.Id;
        }
        else
            throw new ArgumentException("Необходимо создать " +
                                        "Достопримечательность или указать существующего");

        Review newReview = new Review
        {
            Title = createReviewDTO.Title,
            Content = createReviewDTO.Content,
            Rating = createReviewDTO.Rating,
            Author = createReviewDTO.Author,
            AttractionId = attractionID,
            CreatedDate = DateTime.UtcNow
        };

        var createdBook = _reviewRepository.Create(newReview);
        return (MapToReviewDTO(createdBook));
    }

    // <summary>
    // обновляет существующий отзыв
    // </summary>
    // <param name="id">идентификатор обновляемого отзыва</param>
    // <param name="updateReviewDTO">DTO с обновленными данными отзыва</param>
    public ReviewDTO UpdateReview(int id, UpdateReviewDTO updateReviewDTO)
    {
        var review = _reviewRepository.GetById(id);
        if (review == null) return null;

        if (_attractionRepository.Exists(updateReviewDTO.AttractionId))
        {
            review.Title = updateReviewDTO.Title;
            review.Content = updateReviewDTO.Content;
            review.Rating = updateReviewDTO.Rating;
            review.Author = updateReviewDTO.Author;

            var updatesReview = _reviewRepository.Update(review);
            return MapToReviewDTO(updatesReview);
        }
        else
            throw new ArgumentException("Достопримечательность с таким id не была найдена");
    }
    
    // <summary>
    // удаляет отзыв по идентификатору
    // </summary>
    // <param name="id">идентификатор отзыва для удаления</param>
    public bool DeleteReview(int id) => _reviewRepository.Delete(id);

    // <summary>
    // получает отзыв по идентификатору достопримечательности
    // </summary>
    // <param name="attractionId">идентификатор достопримечательности</param>
    public IEnumerable<ReviewDTO> GetByAttraction(int attractionId)
    {
        if (!_attractionRepository.Exists(attractionId))
            return null;

        var reviews = _reviewRepository.GetReviewByAttraction(attractionId);
        return reviews.Select(MapToReviewDTO);
    }
}