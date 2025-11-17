namespace AttractionReviewAPI.DTO;

public class AuthResponseDTO
{
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime ValidTo { get; set; }
    public UserDTO User { get; set; } = new UserDTO();
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
}