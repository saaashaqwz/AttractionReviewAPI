using System.ComponentModel.DataAnnotations;

namespace AttractionReviewAPI;

public class Role
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(20, ErrorMessage = "Название роли не может содержать больше 20 символов")]
    public string Name { get; set; } = null!;

    public List<User> Users { get; set; } = new();
}