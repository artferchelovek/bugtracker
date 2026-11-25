using server.DTOs;
using server.Models;

namespace server.Repositories
{
    public interface ISoftwareRepository
    {
        Task<bool> NameExistsAsync(string name);
        Task<Software?> GetByIdAsync(Guid id);
        Task<Software?> GetByIdWithDetailsAsync(Guid id);
        Task<List<SoftwareDto>> GetSoftwaresAsync(bool myOnly, Guid currentUserId, string? search = null);
        Task<List<Developer>> GetActiveDevelopersAsync();
        Task<TeamMember?> GetTeamMemberByIdAsync(Guid userId);
        Task<List<Developer>> GetDevelopersByIdsAsync(IEnumerable<Guid> ids);
        void Add(Software software);
        Task SaveChangesAsync();
    }
}
