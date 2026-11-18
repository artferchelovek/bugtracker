using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Models;

namespace server.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _context;

        public AuthRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Administrator?> GetAdminByLoginAsync(string login)
        {
            var lower = login.Trim().ToLower();
            return await _context.Administrators
                .FirstOrDefaultAsync(a => a.Login.ToLower() == lower);
        }

        public async Task<TeamMember?> GetMemberByLoginAsync(string login)
        {
            var lower = login.Trim().ToLower();
            return await _context.TeamMembers
                .Include(m => m.RegisteredBy)
                .FirstOrDefaultAsync(m => m.Login.ToLower() == lower);
        }

        public async Task<Administrator?> GetAdminByIdAsync(Guid id)
        {
            return await _context.Administrators
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<TeamMember?> GetMemberByIdAsync(Guid id)
        {
            return await _context.TeamMembers
                .Include(m => m.RegisteredBy)
                .FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
