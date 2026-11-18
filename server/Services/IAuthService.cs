using server.DTOs;

namespace server.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> LoginAsync(LoginDto dto);
        Task<UserDto?> GetCurrentUserAsync(Guid userId, string role);
    }
}
