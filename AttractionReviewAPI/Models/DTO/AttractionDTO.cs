using System.ComponentModel.DataAnnotations;

namespace AttractionReviewAPI.DTO;

public class AttractionDTO
{
    [Key]
    public int Id { get; set; }
    
    [Required (ErrorMessage = "название не может быть пустым")]
    [StringLength(50, ErrorMessage = "название не может содержать больше 50 символов")]
    public string Name { get; set; } = null!;
    
    [StringLength(100, ErrorMessage = "описание не может содержать больше 100 символов")]
    public string? Description { get; set; } 
    
    [Required (ErrorMessage = "место не может быть пустым")]
    [StringLength(50, ErrorMessage = "место не может содержать больше 50 символов")]
    public string Location { get; set; } = null!; 
    
    [StringLength(40, ErrorMessage = "категория не может содержать больше 40 символов")]
    public string? Category { get; set; }
}