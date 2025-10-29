namespace AttractionReviewAPI;

public class Review
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int Rating { get; set; } 
    public string Author { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    
    public int AttractionId { get; set; }
    public Attraction Attraction { get; set; } = null!;
}