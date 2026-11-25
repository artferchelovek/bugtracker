using server.DTOs;

namespace server.Services
{
    public interface ISoftwareService
    {
        Task<SoftwareDto> CreateSoftwareAsync(CreateSoftwareDto dto, Guid currentUserId);
        Task<List<SoftwareDto>> GetSoftwaresAsync(bool myOnly, Guid currentUserId, string? search = null);
        Task<List<UserDto>> GetActiveDevelopersAsync();
        Task<bool> AddToMyListAsync(Guid softwareId, Guid currentUserId);
        Task<SoftwareParamsDto?> GetSoftwareParamsAsync(Guid softwareId);
        Task<ComponentDto> AddComponentAsync(Guid softwareId, CreateNamedItemDto dto, Guid currentUserId);
        Task<ReleaseDto> AddReleaseAsync(Guid softwareId, CreateReleaseDto dto, Guid currentUserId);
        Task<EnvironmentDto> AddEnvironmentAsync(Guid softwareId, CreateNamedItemDto dto, Guid currentUserId);
        Task<ReleaseDto> ToggleReleaseStatusAsync(Guid softwareId, Guid releaseId, Guid currentUserId);
    }
}
