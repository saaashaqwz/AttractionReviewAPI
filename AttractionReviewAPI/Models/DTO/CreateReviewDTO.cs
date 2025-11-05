namespace AttractionReviewAPI.DTO;

public class CreateReviewDTO
{
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public int Rating { get; set; } 
    public string Author { get; set; } = null!;
    
    public List<int>? AttractionIds { get; set; } //существует достопримечательность
    public List<CreateAttractionDTO>? NewAttractions { get; set; } //новая достопримечательность
}