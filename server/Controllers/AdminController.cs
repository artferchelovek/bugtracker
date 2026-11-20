using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.DTOs;
using server.Services;

namespace server.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _service;

        public AdminController(IAdminService service)
        {
            _service = service;
        }

        [HttpGet("users")]
        [ProducesResponseType(typeof(List<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetUsersAsync()
        {
            var users = await _service.GetUsersAsync();
            return Ok(users);
        }

        [HttpPost("users")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> RegisterUserAsync([FromBody] CreateUserDto dto)
        {
            if (!Guid.TryParse(User.FindFirst("sub")?.Value, out var adminId))
            {
                return Unauthorized(new { Message = "Идентификатор администратора не найден в токене." });
            }

            try
            {
                var user = await _service.RegisterUserAsync(dto, adminId);
                return StatusCode(201, user);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("users/{id:guid}/deactivate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeactivateUserAsync(Guid id)
        {
            var success = await _service.DeactivateUserAsync(id);
            if (!success)
            {
                return NotFound(new { Message = "Пользователь не найден." });
            }

            return Ok(new { Message = "Пользователь успешно деактивирован." });
        }
    }
}
