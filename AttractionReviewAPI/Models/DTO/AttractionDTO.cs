namespace AttractionReviewAPI.DTO;

public class AttractionDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; } 
    public string Location { get; set; } = null!; 
    public string? Category { get; set; }
    
    public List<ReviewDTO> Reviews { get; set; } = new();
}