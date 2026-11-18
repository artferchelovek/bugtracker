using server.DTOs;
using server.Models;
using server.Repositories;

namespace server.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _repository;

        public AdminService(IAdminRepository repository)
        {
            _repository = repository;
        }

        public async Task<UserDto> RegisterUserAsync(CreateUserDto dto, Guid adminId)
        {
            var login = dto.Login.Trim();
            var email = dto.Email?.Trim() ?? string.Empty;

            var isTester = string.Equals(dto.Role?.Trim(), "Tester", StringComparison.OrdinalIgnoreCase);
            var isDeveloper = string.Equals(dto.Role?.Trim(), "Developer", StringComparison.OrdinalIgnoreCase);
            if (!isTester && !isDeveloper)
            {
                throw new InvalidOperationException("Роль должна быть Tester или Developer.");
            }

            if (await _repository.LoginExistsAsync(login))
            {
                throw new InvalidOperationException("Пользователь с таким логином уже существует.");
            }

            if (!string.IsNullOrWhiteSpace(email) && await _repository.EmailExistsAsync(email))
            {
                throw new InvalidOperationException("Пользователь с таким email уже существует.");
            }

            var admin = await _repository.GetAdminByIdAsync(adminId)
                ?? throw new InvalidOperationException("Администратор не найден.");

            TeamMember member = isTester
                ? new Tester
                {
                    Id = Guid.NewGuid(),
                    Login = login,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                    FullName = dto.FullName.Trim(),
                    Email = email,
                    Position = dto.Position?.Trim() ?? string.Empty,
                    IsActive = true,
                    RegisteredById = admin.Id
                }
                : new Developer
                {
                    Id = Guid.NewGuid(),
                    Login = login,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                    FullName = dto.FullName.Trim(),
                    Email = email,
                    Position = dto.Position?.Trim() ?? string.Empty,
                    IsActive = true,
                    RegisteredById = admin.Id
                };

            _repository.AddUser(member);
            await _repository.SaveChangesAsync();

            return new UserDto
            {
                Id = member.Id,
                Login = member.Login,
                FullName = member.FullName,
                Email = member.Email,
                Position = member.Position,
                Role = member is Tester ? "Tester" : "Developer",
                IsActive = member.IsActive,
                RegisteredByLogin = admin.Login
            };
        }

        public async Task<List<UserDto>> GetUsersAsync()
        {
            var users = await _repository.GetAllUsersAsync();

            return users.Select(u => new UserDto
            {
                Id = u.Id,
                Login = u.Login,
                FullName = u.FullName,
                Email = u.Email,
                Position = u.Position,
                Role = u is Tester ? "Tester" : "Developer",
                IsActive = u.IsActive,
                RegisteredByLogin = u.RegisteredBy?.Login
            }).ToList();
        }

        public async Task<bool> DeactivateUserAsync(Guid id)
        {
            var user = await _repository.GetUserByIdAsync(id);
            if (user == null)
            {
                return false;
            }

            user.IsActive = false;
            await _repository.SaveChangesAsync();

            return true;
        }
    }
}
