using AttractionReviewAPI.DTO;

namespace AttractionReviewAPI.Services;

public interface IAttractionService
{
    IEnumerable<AttractionDTO> GetAllAttractions();
    AttractionDTO GetByIdAttraction(int id);
    AttractionDTO CreateAttraction(CreateAttractionDTO createAttractionDTO);
    AttractionDTO UpdateAttraction(int id, CreateAttractionDTO updateAttractionDTO);
    bool DeleteAttraction(int id);
}