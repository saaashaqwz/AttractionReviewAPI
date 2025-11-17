using System.ComponentModel.DataAnnotations;

namespace AttractionReviewAPI.DTO;

public class LoginRequestDTO
{
    [Required (ErrorMessage = "Необходимо ввести email или имя пользователя")]
    public string EmailOrUsername { get; set; } = null!;
    
    [Required (ErrorMessage = "Необходимо ввести пароль")]
    [MinLength(8, ErrorMessage = "Пароль должен содержать от 8 символов")]
    public string Password { get; set; } = null!;
    
    public int RoleId { get; set; }
}