using System.ComponentModel.DataAnnotations;

namespace AttractionReviewAPI;

public class Review
{
    [Key]
    public int Id { get; set; }
    
    [Required (ErrorMessage = "заголовок не может быть пустым")]
    [StringLength(30, ErrorMessage = "заголовок не может содержать больше 30 символов")]
    public string Title { get; set; } = null!;
    
    [Required (ErrorMessage = "отзыв не может быть пустым")]
    [MinLength(20, ErrorMessage = "отзыв должен содержать от 20 символов")]
    [StringLength(500, ErrorMessage = "отзыв не может содержать больше 500 символов")]
    public string Content { get; set; } = null!;
    
    [Required (ErrorMessage = "отзыв не может быть без оценки")]
    [Range(1, 5, ErrorMessage = "оценка должна быть от 1 до 5")]
    public int Rating { get; set; } 
    
    [Required (ErrorMessage = "автор не может быть пустым")]
    [MinLength(20, ErrorMessage = "автор должен содержать от 20 символов")]
    public string Author { get; set; } = null!;
    
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    
    public int AttractionId { get; set; }
    public Attraction Attraction { get; set; } = null!;
}