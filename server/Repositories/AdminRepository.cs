using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Models;

namespace server.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly AppDbContext _context;

        public AdminRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> LoginExistsAsync(string login)
        {
            var lower = login.Trim().ToLower();
            var inMembers = await _context.TeamMembers.AnyAsync(m => m.Login.ToLower() == lower);
            var inAdmins = await _context.Administrators.AnyAsync(a => a.Login.ToLower() == lower);

            return inMembers || inAdmins;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            var lower = email.Trim().ToLower();
            return await _context.TeamMembers.AnyAsync(m => m.Email.ToLower() == lower);
        }

        public async Task<Administrator?> GetFirstAdminAsync()
        {
            return await _context.Administrators.FirstOrDefaultAsync();
        }

        public async Task<Administrator?> GetAdminByIdAsync(Guid id)
        {
            return await _context.Administrators.FirstOrDefaultAsync(a => a.Id == id);
        }

        public void AddUser(TeamMember member)
        {
            _context.TeamMembers.Add(member);
        }

        public async Task<List<TeamMember>> GetAllUsersAsync()
        {
            return await _context.TeamMembers
                .Include(m => m.RegisteredBy)
                .OrderBy(m => m.FullName)
                .ToListAsync();
        }

        public async Task<TeamMember?> GetUserByIdAsync(Guid id)
        {
            return await _context.TeamMembers
                .Include(m => m.RegisteredBy)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
