using AttractionReviewAPI.DTO;
using Microsoft.AspNetCore.Identity.Data;

namespace AttractionReviewAPI.Services;

public interface IAuthService
{
    AuthResponseDTO Register(RegisterRequestDTO createUserRequest);
    AuthResponseDTO Login(LoginRequestDTO loginRequest);
    AuthResponseDTO RefreshToken(string refreshToken);
    bool ValidateToken(string token);
}