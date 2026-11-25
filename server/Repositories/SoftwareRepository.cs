using Microsoft.EntityFrameworkCore;
using server.Data;
using server.DTOs;
using server.Models;
using server.Models.Enums;

namespace server.Repositories
{
    public class SoftwareRepository : ISoftwareRepository
    {
        private readonly AppDbContext _context;

        public SoftwareRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> NameExistsAsync(string name)
        {
            return await _context.Softwares.AnyAsync(s => s.Name.ToLower() == name.ToLower());
        }

        public async Task<Software?> GetByIdAsync(Guid id)
        {
            return await _context.Softwares.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Software?> GetByIdWithDetailsAsync(Guid id)
        {
            return await _context.Softwares
                .AsSplitQuery()
                .Include(s => s.Owner)
                .Include(s => s.Components)
                .Include(s => s.Releases)
                .Include(s => s.Environments)
                .Include(s => s.Members)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<SoftwareDto>> GetSoftwaresAsync(bool myOnly, Guid currentUserId, string? search = null)
        {
            var query = _context.Softwares.AsNoTracking();

            if (myOnly)
            {
                query = query.Where(s => s.OwnerId == currentUserId || s.Members.Any(m => m.Id == currentUserId));
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                var trimmedSearch = search.Trim().ToLower();
                query = query.Where(s => s.Name.ToLower().Contains(trimmedSearch) || (s.Owner != null && s.Owner.FullName.ToLower().Contains(trimmedSearch)));
            }

            return await query
                .OrderBy(s => s.Name)
                .Select(s => new SoftwareDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    ContactEmail = s.ContactEmail,
                    TechStack = s.TechStack,
                    OwnerId = s.OwnerId,
                    OwnerName = s.Owner != null ? s.Owner.FullName : string.Empty,
                    IsInMyList = s.OwnerId == currentUserId || s.Members.Any(m => m.Id == currentUserId),
                    ComponentsCount = s.Components.Count(),
                    ReleasesCount = s.Releases.Count(),
                    EnvironmentsCount = s.Environments.Count(),
                    OpenTicketsCount = s.Tickets.Count(t => t.Status != TicketStatus.CLOSED && t.Status != TicketStatus.DRAFT)
                })
                .ToListAsync();
        }

        public async Task<List<Developer>> GetActiveDevelopersAsync()
        {
            return await _context.Developers
                .AsNoTracking()
                .Where(d => d.IsActive)
                .OrderBy(d => d.FullName)
                .ToListAsync();
        }

        public async Task<TeamMember?> GetTeamMemberByIdAsync(Guid userId)
        {
            return await _context.TeamMembers
                .Include(m => m.ParticipatingSoftwares)
                .FirstOrDefaultAsync(m => m.Id == userId);
        }

        public async Task<List<Developer>> GetDevelopersByIdsAsync(IEnumerable<Guid> ids)
        {
            return await _context.Developers
                .Where(d => ids.Contains(d.Id) && d.IsActive)
                .ToListAsync();
        }

        public void Add(Software software)
        {
            _context.Softwares.Add(software);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
