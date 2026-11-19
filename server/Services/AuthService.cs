using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using server.DTOs;
using server.Models;
using server.Repositories;

namespace server.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _repository;
        private readonly IConfiguration _configuration;

        public AuthService(IAuthRepository repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Login) || string.IsNullOrWhiteSpace(dto.Password))
            {
                return null;
            }

            var login = dto.Login.Trim();

            var member = await _repository.GetMemberByLoginAsync(login);
            if (member != null)
            {
                if (!member.IsActive)
                {
                    return null;
                }

                if (!BCrypt.Net.BCrypt.Verify(dto.Password, member.PasswordHash))
                {
                    return null;
                }

                var role = member is Tester ? "Tester" : "Developer";
                var token = GenerateJwtToken(member.Id, member.Login, role, member.Email);

                return new LoginResponseDto
                {
                    Token = token,
                    User = new UserDto
                    {
                        Id = member.Id,
                        Login = member.Login,
                        FullName = member.FullName,
                        Email = member.Email,
                        Position = member.Position,
                        Role = role,
                        IsActive = member.IsActive,
                        RegisteredByLogin = member.RegisteredBy?.Login
                    }
                };
            }

            var admin = await _repository.GetAdminByLoginAsync(login);
            if (admin != null)
            {
                if (!BCrypt.Net.BCrypt.Verify(dto.Password, admin.PasswordHash))
                {
                    return null;
                }

                var token = GenerateJwtToken(admin.Id, admin.Login, "Admin", admin.Email);

                return new LoginResponseDto
                {
                    Token = token,
                    User = new UserDto
                    {
                        Id = admin.Id,
                        Login = admin.Login,
                        FullName = "Administrator",
                        Email = admin.Email,
                        Position = "System Administrator",
                        Role = "Admin",
                        IsActive = true,
                        RegisteredByLogin = null
                    }
                };
            }

            return null;
        }

        public async Task<UserDto?> GetCurrentUserAsync(Guid userId, string role)
        {
            if (role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                var admin = await _repository.GetAdminByIdAsync(userId);
                if (admin == null)
                {
                    return null;
                }

                return new UserDto
                {
                    Id = admin.Id,
                    Login = admin.Login,
                    FullName = "Administrator",
                    Email = admin.Email,
                    Position = "System Administrator",
                    Role = "Admin",
                    IsActive = true,
                    RegisteredByLogin = null
                };
            }

            var member = await _repository.GetMemberByIdAsync(userId);
            if (member == null || !member.IsActive)
            {
                return null;
            }

            return new UserDto
            {
                Id = member.Id,
                Login = member.Login,
                FullName = member.FullName,
                Email = member.Email,
                Position = member.Position,
                Role = member is Tester ? "Tester" : "Developer",
                IsActive = member.IsActive,
                RegisteredByLogin = member.RegisteredBy?.Login
            };
        }

        private string GenerateJwtToken(Guid id, string login, string role, string email)
        {
            var secretKey = _configuration["Jwt:SecretKey"]
                ?? _configuration["Jwt:Key"]
                ?? throw new InvalidOperationException("JWT SecretKey is not configured.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim("sub", id.ToString()),
                new Claim("login", login),
                new Claim("role", role),
                new Claim("email", email)
            };

            var expirationMinutes = int.TryParse(_configuration["Jwt:AccessTokenExpirationMinutes"], out var minutes)
                ? minutes
                : 120;

            var jwt = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
