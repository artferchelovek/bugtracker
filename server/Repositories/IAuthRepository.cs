using server.Models;

namespace server.Repositories
{
    public interface IAuthRepository
    {
        Task<Administrator?> GetAdminByLoginAsync(string login);
        Task<TeamMember?> GetMemberByLoginAsync(string login);
        Task<Administrator?> GetAdminByIdAsync(Guid id);
        Task<TeamMember?> GetMemberByIdAsync(Guid id);
    }
}
