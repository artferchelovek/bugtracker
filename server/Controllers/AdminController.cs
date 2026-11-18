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
        public async Task<IActionResult> GetUsersAsync()
        {
            var users = await _service.GetUsersAsync();
            return Ok(users);
        }

        [HttpPost("users")]
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
