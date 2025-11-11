using AttractionReviewAPI.DTO;
using AttractionReviewAPI.Repositories;

namespace AttractionReviewAPI.Services;

public class AttractionService : IAttractionService
{
    private readonly IAttractionRepository _attractionRepository;
    private readonly IReviewRepository _reviewRepository;

    public AttractionService(IAttractionRepository attractionRepo, IReviewRepository reviewRepo)
    {
        _attractionRepository = attractionRepo;
        _reviewRepository = reviewRepo;
    }

    // <summary>
    // преобразует объект Attraction в AttractionDTO
    // </summary>
    // <param name="review">объект достопримечательности из базы данных</param>
    private static AttractionDTO MapToAttractionDTO(Attraction attraction)
    {
        return new AttractionDTO
        {
            Id = attraction.Id,
            Name = attraction.Name,
            Description = attraction.Description,
            Location = attraction.Location,
            Category = attraction.Category
        };
    }
    
    // <summary>
    // получает все достопримечательности
    // </summary>
    public IEnumerable<AttractionDTO> GetAllAttractions()
    {
        var attractions = _attractionRepository.GetAll();
        return attractions.Select(MapToAttractionDTO);
    }

    // <summary>
    // получает достопримечательность по идентификатору
    // </summary>
    // <param name="id">идентификатор достопримечательности</param>
    public AttractionDTO GetByIdAttraction(int id)
    {
        var attraction = _attractionRepository.GetById(id);
        return attraction == null ? null : MapToAttractionDTO(attraction);
    }

    
    // <summary>
    // создает новую достопримечательность
    // </summary>
    // <param name="createAttractionDTO">DTO с данными для создания достопримечательности</param>
    public AttractionDTO CreateAttraction(CreateAttractionDTO createAttractionDTO)
    {
        if (_attractionRepository.AttractionExist(createAttractionDTO.Name))
            throw new ArgumentException($"Достопримечательность с именем {createAttractionDTO.Name} уже существует");

        var newAttraction = new Attraction
        {
            Name = createAttractionDTO.Name,
            Description = createAttractionDTO.Description,
            Location = createAttractionDTO.Location,
            Category = createAttractionDTO.Category
        };

        var createdAttraction = _attractionRepository.Create(newAttraction);
        return MapToAttractionDTO(createdAttraction);
    }

    // <summary>
    // обновляет существующая достопримечательность
    // </summary>
    // <param name="id">идентификатор обновляемой достопримечательности</param>
    // <param name="updateAttractionDTO">DTO с обновленными данными достопримечательности</param>
    public AttractionDTO UpdateAttraction(int id, CreateAttractionDTO updateAttractionDTO)
    {
        var attraction = _attractionRepository.GetById(id);
        if (attraction == null) 
            return null;

        if (attraction.Name != updateAttractionDTO.Name &&
            _attractionRepository.AttractionExist(updateAttractionDTO.Name))
            throw new ArgumentException($"Достопримечательность с именем {updateAttractionDTO.Name} уже существует");

        attraction.Name = updateAttractionDTO.Name;
        attraction.Description = updateAttractionDTO.Description;
        attraction.Location = updateAttractionDTO.Location;
        attraction.Category = updateAttractionDTO.Category;

        var updatedAttraction = _attractionRepository.Update(attraction);
        return MapToAttractionDTO(updatedAttraction);
    }
    
    // <summary>
    // удаляет достопримечательность по идентификатору
    // </summary>
    // <param name="id">идентификатор достопримечательности для удаления</param>
    public bool DeleteAttraction(int id) => _attractionRepository.Delete(id);
}