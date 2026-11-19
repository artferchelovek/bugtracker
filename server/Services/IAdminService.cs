using server.DTOs;

namespace server.Services
{
    public interface IAdminService
    {
        Task<UserDto> RegisterUserAsync(CreateUserDto dto, Guid adminId);
        Task<List<UserDto>> GetUsersAsync();
        Task<bool> DeactivateUserAsync(Guid id);
    }
}
