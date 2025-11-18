using AttractionReviewAPI.DTO;
using AttractionReviewAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttractionReviewAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _logger = logger;
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpPost("Register")]
    public ActionResult<AuthResponseDTO> Register([FromBody] RegisterRequestDTO registerRequestDTO)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(
                    new AuthResponseDTO
                    {
                        Success = false,
                        ErrorMessage = "Входные данные неверные"
                    });
            }

            _logger.LogWarning("Пользователь {username} не прошел аутентификацию. Данные неверные",
                registerRequestDTO.Username);

            var result = _authService.Register(registerRequestDTO);
            if (!result.Success)
            {
                return Unauthorized(result);
            }

            return CreatedAtAction(nameof(Register), result);
        }
        catch (Exception ex)
        {
            return BadRequest(
                new AuthResponseDTO
                {
                    Success = false,
                    ErrorMessage = ex.Message
                });
        }
    }

    [AllowAnonymous]
    [HttpPost("Login")]
    public ActionResult<AuthResponseDTO> Login([FromBody] LoginRequestDTO loginRequestDTO)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(
                    new AuthResponseDTO
                    {
                        Success = false,
                        ErrorMessage = "Входные данные неверные"
                    });
            }

            _logger.LogInformation("Пользователь {userName} запросил аутентификацию",
                loginRequestDTO.EmailOrUsername);

            AuthResponseDTO result = _authService.Login(loginRequestDTO);

            if (!result.Success)
            {
                _logger.LogWarning("Пользователь {username} не смог аутентифицироваться",
                    loginRequestDTO.EmailOrUsername);
                return Unauthorized(new AuthResponseDTO
                {
                    Success = false,
                    ErrorMessage = "Авторизация не прошла"
                });
            }

            _logger.LogInformation("Пользователь {username} успешно аутентифицировался",
                loginRequestDTO.EmailOrUsername);

            var response = new AuthResponseDTO
            {
                Success = true,
                Token = result.Token,
                RefreshToken = result.RefreshToken,
                ValidTo = result.ValidTo,
                User = result.User
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Произошла ошибка аутентификации со стороны сервера {message}", ex.Message);
            return StatusCode(500, new AuthResponseDTO
            {
                ErrorMessage = "Непредвиденная ошибка",
                Success = false
            });
        }
    }

    [AllowAnonymous]
    [HttpPost("Refresh")]
    public ActionResult<AuthResponseDTO> Refresh(string refreshToken)
    {
        try
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(refreshToken))
            {
                return BadRequest(
                    new AuthResponseDTO
                    {
                        Success = false,
                        ErrorMessage = "Refresh token обязателен"
                    });
            }

            _logger.LogInformation("Запрос на обновление токена");

            var result = _authService.RefreshToken(refreshToken);

            if (!result.Success)
            {
                _logger.LogWarning("Обновление токена не удалось: {error}", result.ErrorMessage);
                return Unauthorized(result);
            }

            _logger.LogInformation("Токен успешно обновлен для пользователя {userId}", result.User.Id);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError("Ошибка при обновлении токена: {message}", ex.Message);
            return StatusCode(500, new AuthResponseDTO
            {
                Success = false,
                ErrorMessage = "Ошибка при обновлении токена"
            });
        }
    }
}
