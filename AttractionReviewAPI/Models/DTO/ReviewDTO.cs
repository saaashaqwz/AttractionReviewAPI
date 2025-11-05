namespace AttractionReviewAPI.DTO;

public class ReviewDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public int Rating { get; set; } 
    public string Author { get; set; } = null!;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    
    public int AttractionId { get; set; }
    public AttractionDTO? Attraction { get; set; }
}