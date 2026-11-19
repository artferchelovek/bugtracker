using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.DTOs;
using server.Services;

namespace server.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto? dto)
        {
            if (dto == null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            var response = await _authService.LoginAsync(dto);
            if (response == null)
            {
                return Unauthorized(new { Message = "Неверный логин или пароль, либо аккаунт деактивирован." });
            }

            return Ok(response);
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetMeAsync()
        {
            var userIdClaim = User.FindFirst("sub")?.Value;
            var roleClaim = User.FindFirst("role")?.Value;

            if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId) || roleClaim == null)
            {
                return Unauthorized();
            }

            var user = await _authService.GetCurrentUserAsync(userId, roleClaim);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
    }
}
