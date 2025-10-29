namespace AttractionReviewAPI;

public class Attraction
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty; 
    public string Location { get; set; } = string.Empty; 
    public string Category { get; set; } = string.Empty;
    
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}