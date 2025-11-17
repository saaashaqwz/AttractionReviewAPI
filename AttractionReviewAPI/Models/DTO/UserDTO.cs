using System.ComponentModel.DataAnnotations;

namespace AttractionReviewAPI.DTO;

public class UserDTO
{
    [Key]
    public int Id { get; set; }
    
    [Required (ErrorMessage = "Имя пользователя не может быть пустым")]
    [StringLength(50, ErrorMessage = "Имя пользователя не может содержать больше 50 символов")]
    public string Username { get; set; }
    
    [Required (ErrorMessage = "Email не может быть пустым")]
    [EmailAddress]
    [StringLength(100, ErrorMessage = "Email не может содержать больше 100 символов")]
    public string Email { get; set; }
    
    public string RoleName  { get; set; }
}