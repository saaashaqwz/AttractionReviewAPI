using System.ComponentModel.DataAnnotations;

namespace AttractionReviewAPI.DTO;

public class RegisterRequestDTO
{
    [Required (ErrorMessage = "Имя пользователя не может быть пустым")]
    [StringLength(50, ErrorMessage = "Имя пользователя не может содержать больше 50 символов")]
    public string Username { get; set; }
    
    [Required (ErrorMessage = "Email не может быть пустым")]
    [StringLength(100, ErrorMessage = "Email не может содержать больше 100 символов")]
    public string Email { get; set; } = null!;
    
    [Required (ErrorMessage = "Пароль не может быть пустым")]
    [MinLength(8, ErrorMessage = "Пароль должен содержать от 8 символов")]
    public string Password { get; set; } = null!;

    public int RoleId { get; set; } = 2;
}