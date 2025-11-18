using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace AttractionReviewAPI;

[Index(nameof(Username), IsUnique = true)]
[Index(nameof(Email), IsUnique = true)]
public class User
{
    [Key]
    public int Id { get; set; }
    
    [Required (ErrorMessage = "Имя пользователя не может быть пустым")]
    [StringLength(50, ErrorMessage = "Имя пользователя не может содержать больше 50 символов")]
    public string Username { get; set; }
        
    [Required (ErrorMessage = "Email не может быть пустым")]
    [EmailAddress]
    [StringLength(100, ErrorMessage = "Email не может содержать больше 100 символов")]
    public string Email { get; set; } = string.Empty;
        
    [Required (ErrorMessage = "Пароль не может быть пустым")]
    public string PasswordHash { get; set; } = string.Empty;
    
    public bool IsActive { get; set; } = true;
    
    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;
}