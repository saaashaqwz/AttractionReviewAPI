namespace AttractionReviewAPI.DTO;

public class CreateAttractionDTO
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string Location { get; set; } = null!; 
    public string? Category { get; set; }
}