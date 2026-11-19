using server.Models;

namespace server.Repositories
{
    public interface IAdminRepository
    {
        Task<bool> LoginExistsAsync(string login);
        Task<bool> EmailExistsAsync(string email);
        Task<Administrator?> GetFirstAdminAsync();
        Task<Administrator?> GetAdminByIdAsync(Guid id);
        void AddUser(TeamMember member);
        Task<List<TeamMember>> GetAllUsersAsync();
        Task<TeamMember?> GetUserByIdAsync(Guid id);
        Task SaveChangesAsync();
    }
}
