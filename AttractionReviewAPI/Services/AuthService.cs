using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AttractionReviewAPI.DTO;
using AttractionReviewAPI.Repositories;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AttractionReviewAPI.Services;

public class AuthService : IAuthService
{
    private readonly JwtConfiguration _jwtConfiguration;
    private readonly IUserRepository _userRepository;
    private readonly APIDBContext _context;
    
    public AuthService(IOptions<JwtConfiguration> jwtConfiguration, IUserRepository userRepository)
    {
        _jwtConfiguration = jwtConfiguration.Value;
        _userRepository = userRepository;
    }
    
    public AuthResponseDTO Register(RegisterRequestDTO registerRequestDTO)
    {
        try
        {
            var role = _userRepository.RoleExist(registerRequestDTO.RoleId);
            if (role == null)
                throw new ArgumentException("Роль с таким id не существует");
            
            var userByEmail = _userRepository.ExistUser(registerRequestDTO.Email);
            var userByUsername = _userRepository.ExistUser(registerRequestDTO.Username);

            if (userByEmail != null || userByUsername != null)
            {
                return new AuthResponseDTO
                {
                    Success = false,
                    ErrorMessage = "Такой пользователь уже существует"
                };
            }
            else
            {
                User newUser = new User
                {
                    Username = registerRequestDTO.Username,
                    Email = registerRequestDTO.Email,
                    PasswordHash = GetHashPassword(registerRequestDTO.Password),
                    IsActive = true,
                    RoleId = registerRequestDTO.RoleId
                };

                var addedUser = _userRepository.AddUser(newUser);

                return new AuthResponseDTO
                {
                    Success = true,
                    Token = GenerateJwtToken(addedUser),
                    RefreshToken = GenerateRefreshToken(),
                    ValidTo = DateTime.UtcNow
                        .AddMinutes(_jwtConfiguration.ExpirateAtInMinutes),
                    User = new UserDTO
                    {
                        Id = addedUser.Id,
                        Username = addedUser.Username,
                        Email = addedUser.Email,
                        RoleName = addedUser.Role.Name
                    }
                };

            }
        }
        catch(Exception ex)
        {
            return new AuthResponseDTO
            {
                Success = false,
                ErrorMessage = ex.Message
            };
        }
    }

    public AuthResponseDTO Login(LoginRequestDTO loginRequestDTO)
    {
        try
        {
            var user = _userRepository.ExistUser(loginRequestDTO.EmailOrUsername);
            var role = _userRepository.RoleExist(loginRequestDTO.RoleId);
            
            if (role == null)
                return new AuthResponseDTO
                {
                    Success = false,
                    ErrorMessage = "Такой роли не существует"
                };
            
            if (user == null)
                return new AuthResponseDTO
                {
                    Success = false,
                    ErrorMessage = "Такого пользователя не зарегистрирован"
                };

            if (user.PasswordHash != GetHashPassword(loginRequestDTO.Password))
                return new AuthResponseDTO
                {
                    Success = false,
                    ErrorMessage = "Неверный пароль"
                };
            else
            {
                var token = GenerateJwtToken(user);
                var refrashToken = GenerateRefreshToken();

                return new AuthResponseDTO
                {
                    Success = true,
                    Token = token,
                    RefreshToken = refrashToken,
                    ValidTo = DateTime.UtcNow
                        .AddMinutes(_jwtConfiguration.ExpirateAtInMinutes),
                    User = new UserDTO
                    {
                        Id = user.Id,
                        Username = user.Username,
                        Email = user.Email, 
                        RoleName = user.Role.Name
                    }
                };
            }
        }
        catch(Exception ex)
        {
            return new AuthResponseDTO
            {
                Success = false,
                ErrorMessage = $"Некорретные данные + {ex.Message}"
            };
        }
    }

    public AuthResponseDTO RefreshToken(string refreshToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return new AuthResponseDTO
                {
                    Success = false,
                    ErrorMessage = "Refresh token не найден"
                };
            }

            var user = _userRepository.GetUserById(1);
    
            if (user == null || !user.IsActive)
            {
                return new AuthResponseDTO
                {
                    Success = false,
                    ErrorMessage = "Пользователя не существует"
                };
            }

            var newJwtToken = GenerateJwtToken(user);
            var newRefreshToken = GenerateRefreshToken();

            return new AuthResponseDTO
            {
                Success = true,
                Token = newJwtToken,
                RefreshToken = newRefreshToken,
                ValidTo = DateTime.UtcNow.AddMinutes(_jwtConfiguration.ExpirateAtInMinutes),
                User = new UserDTO
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    RoleName = user.Role?.Name ?? "User"
                }
            };
        }
        catch (Exception ex)
        {
            return new AuthResponseDTO
            {
                Success = false,
                ErrorMessage = $"Ошибка при обновлении токена: {ex.Message}"
            };
        }
    }

    public bool ValidateToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfiguration.SecretKey);

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _jwtConfiguration.Issuer,
                ValidateAudience = true,
                ValidAudience = _jwtConfiguration.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            return validatedToken != null;
        }
        catch (Exception ex) { return false; }
    }
    
    private string GetHashPassword(string password)
    {
        byte[] bytepass = Encoding.ASCII.GetBytes(password);
        var hashBytes = SHA256.HashData(bytepass);
        
        return Convert.ToBase64String(hashBytes);
    }
    
    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtConfiguration.SecretKey);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role?.Name ?? "User")
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtConfiguration.ExpirateAtInMinutes),
            Issuer = _jwtConfiguration.Issuer,
            Audience = _jwtConfiguration.Audience,
            SigningCredentials = new SigningCredentials
            (
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            )
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rnd = System.Security.Cryptography.RandomNumberGenerator.Create();
        rnd.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}