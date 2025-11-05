namespace AttractionReviewAPI;

public class Attraction
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string Location { get; set; } = null!;
    public string? Category { get; set; }
    
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}