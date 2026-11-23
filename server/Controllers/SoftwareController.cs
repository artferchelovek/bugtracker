using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.DTOs;
using server.Services;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SoftwareController : ControllerBase
    {
        private readonly ISoftwareService _service;

        public SoftwareController(ISoftwareService service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<SoftwareDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetSoftwaresAsync([FromQuery] bool myOnly = false)
        {
            var userId = GetCurrentUserId();
            var result = await _service.GetSoftwaresAsync(myOnly, userId);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Tester")]
        [ProducesResponseType(typeof(SoftwareDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateSoftwareAsync([FromBody] CreateSoftwareDto dto)
        {
            var userId = GetCurrentUserId();
            try
            {
                var result = await _service.CreateSoftwareAsync(dto, userId);
                return StatusCode(StatusCodes.Status201Created, result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { Message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("{id:guid}/my-list")]
        [Authorize(Roles = "Tester,Developer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddToMyListAsync(Guid id)
        {
            var userId = GetCurrentUserId();
            try
            {
                var success = await _service.AddToMyListAsync(id, userId);
                if (!success)
                {
                    return NotFound(new { Message = "Программное обеспечение или пользователь не найдены." });
                }
                return Ok(new { Message = "Проект успешно добавлен в список моих проектов." });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { Message = ex.Message });
            }
        }

        [HttpGet("{id:guid}/params")]
        [ProducesResponseType(typeof(SoftwareParamsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSoftwareParamsAsync(Guid id)
        {
            var result = await _service.GetSoftwareParamsAsync(id);
            if (result == null)
            {
                return NotFound(new { Message = "Программное обеспечение не найдено." });
            }
            return Ok(result);
        }

        [HttpPost("{id:guid}/components")]
        [Authorize(Roles = "Tester")]
        [ProducesResponseType(typeof(ComponentDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddComponentAsync(Guid id, [FromBody] CreateNamedItemDto dto)
        {
            var userId = GetCurrentUserId();
            try
            {
                var result = await _service.AddComponentAsync(id, dto, userId);
                return StatusCode(StatusCodes.Status201Created, result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { Message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("{id:guid}/releases")]
        [Authorize(Roles = "Tester")]
        [ProducesResponseType(typeof(ReleaseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddReleaseAsync(Guid id, [FromBody] CreateReleaseDto dto)
        {
            var userId = GetCurrentUserId();
            try
            {
                var result = await _service.AddReleaseAsync(id, dto, userId);
                return StatusCode(StatusCodes.Status201Created, result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { Message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("{id:guid}/environments")]
        [Authorize(Roles = "Tester")]
        [ProducesResponseType(typeof(EnvironmentDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddEnvironmentAsync(Guid id, [FromBody] CreateNamedItemDto dto)
        {
            var userId = GetCurrentUserId();
            try
            {
                var result = await _service.AddEnvironmentAsync(id, dto, userId);
                return StatusCode(StatusCodes.Status201Created, result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { Message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("{id:guid}/releases/{releaseId:guid}/toggle")]
        [Authorize(Roles = "Tester")]
        [ProducesResponseType(typeof(ReleaseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ToggleReleaseStatusAsync(Guid id, Guid releaseId)
        {
            var userId = GetCurrentUserId();
            try
            {
                var result = await _service.ToggleReleaseStatusAsync(id, releaseId, userId);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { Message = ex.Message });
            }
        }

        private Guid GetCurrentUserId()
        {
            var claimValue = User.FindFirst("sub")?.Value;
            if (Guid.TryParse(claimValue, out var userId))
            {
                return userId;
            }
            return Guid.Empty;
        }
    }
}
