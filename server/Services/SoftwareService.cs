using server.DTOs;
using server.Models;
using server.Repositories;
using Environment = server.Models.Environment;

namespace server.Services
{
    public class SoftwareService : ISoftwareService
    {
        private readonly ISoftwareRepository _repository;

        public SoftwareService(ISoftwareRepository repository)
        {
            _repository = repository;
        }

        public async Task<SoftwareDto> CreateSoftwareAsync(CreateSoftwareDto dto, Guid currentUserId)
        {
            var creator = await _repository.GetTeamMemberByIdAsync(currentUserId);
            if (creator == null || !creator.IsActive)
            {
                throw new UnauthorizedAccessException("Пользователь не найден или деактивирован.");
            }

            if (creator is not Tester tester)
            {
                throw new UnauthorizedAccessException("Только тестировщик может создавать программное обеспечение.");
            }

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                throw new ArgumentException("Название программного обеспечения обязательно.");
            }

            var trimmedName = dto.Name.Trim();
            if (trimmedName.Length > 200)
            {
                throw new ArgumentException("Название не может превышать 200 символов.");
            }

            if (await _repository.NameExistsAsync(trimmedName))
            {
                throw new InvalidOperationException("Программное обеспечение с таким названием уже существует.");
            }

            var software = new Software
            {
                Id = Guid.NewGuid(),
                Name = trimmedName,
                ContactEmail = dto.ContactEmail?.Trim() ?? string.Empty,
                TechStack = dto.TechStack?.Trim() ?? string.Empty,
                OwnerId = currentUserId,
                Owner = tester
            };

            software.Members.Add(tester);

            if (dto.DeveloperIds != null && dto.DeveloperIds.Count > 0)
            {
                var distinctDevIds = dto.DeveloperIds.Distinct().ToList();
                var developers = await _repository.GetDevelopersByIdsAsync(distinctDevIds);
                if (developers.Count != distinctDevIds.Count)
                {
                    throw new ArgumentException("Один или несколько указанных разработчиков не найдены или деактивированы.");
                }

                foreach (var dev in developers)
                {
                    if (!software.Members.Any(m => m.Id == dev.Id))
                    {
                        software.Members.Add(dev);
                    }
                }
            }

            var components = Normalize(dto.Components, 100, "Компоненты");
            foreach (var compName in components)
            {
                software.Components.Add(new Component
                {
                    Id = Guid.NewGuid(),
                    SoftwareId = software.Id,
                    Name = compName
                });
            }

            var releases = Normalize(dto.Releases, 100, "Релизы");
            foreach (var relTag in releases)
            {
                software.Releases.Add(new Release
                {
                    Id = Guid.NewGuid(),
                    SoftwareId = software.Id,
                    VersionTag = relTag,
                    IsReleased = false
                });
            }

            var environments = Normalize(dto.Environments, 100, "Окружения");
            foreach (var envName in environments)
            {
                software.Environments.Add(new Environment
                {
                    Id = Guid.NewGuid(),
                    SoftwareId = software.Id,
                    Name = envName
                });
            }

            _repository.Add(software);
            await _repository.SaveChangesAsync();

            return new SoftwareDto
            {
                Id = software.Id,
                Name = software.Name,
                ContactEmail = software.ContactEmail,
                TechStack = software.TechStack,
                OwnerId = software.OwnerId,
                OwnerName = tester.FullName,
                IsInMyList = true,
                ComponentsCount = software.Components.Count,
                ReleasesCount = software.Releases.Count,
                EnvironmentsCount = software.Environments.Count,
                OpenTicketsCount = 0
            };
        }

        public async Task<List<SoftwareDto>> GetSoftwaresAsync(bool myOnly, Guid currentUserId, string? search = null)
        {
            return await _repository.GetSoftwaresAsync(myOnly, currentUserId, search);
        }

        public async Task<List<UserDto>> GetActiveDevelopersAsync()
        {
            var developers = await _repository.GetActiveDevelopersAsync();
            return developers.Select(d => new UserDto
            {
                Id = d.Id,
                Login = d.Login,
                FullName = d.FullName,
                Email = d.Email,
                Position = d.Position,
                Role = nameof(Developer),
                IsActive = d.IsActive,
                RegisteredByLogin = null
            }).ToList();
        }

        public async Task<bool> AddToMyListAsync(Guid softwareId, Guid currentUserId)
        {
            var software = await _repository.GetByIdWithDetailsAsync(softwareId);
            if (software == null)
            {
                return false;
            }

            var member = await _repository.GetTeamMemberByIdAsync(currentUserId);
            if (member == null || !member.IsActive)
            {
                throw new UnauthorizedAccessException("Пользователь не найден или деактивирован.");
            }

            if (!software.Members.Any(m => m.Id == currentUserId))
            {
                software.Members.Add(member);
                await _repository.SaveChangesAsync();
            }

            return true;
        }

        public async Task<SoftwareParamsDto?> GetSoftwareParamsAsync(Guid softwareId)
        {
            var software = await _repository.GetByIdWithDetailsAsync(softwareId);
            if (software == null)
            {
                return null;
            }

            var participatingDevs = software.Members
                .OfType<Developer>()
                .Where(d => d.IsActive)
                .Select(d => new UserDto
                {
                    Id = d.Id,
                    Login = d.Login,
                    FullName = d.FullName,
                    Email = d.Email,
                    Position = d.Position,
                    Role = "Developer",
                    IsActive = d.IsActive
                })
                .OrderBy(d => d.FullName)
                .ToList();

            return new SoftwareParamsDto
            {
                SoftwareId = software.Id,
                SoftwareName = software.Name,
                Components = software.Components
                    .OrderBy(c => c.Name)
                    .Select(c => new ComponentDto { Id = c.Id, Name = c.Name })
                    .ToList(),
                Releases = software.Releases
                    .OrderBy(r => r.VersionTag)
                    .Select(r => new ReleaseDto { Id = r.Id, VersionTag = r.VersionTag, IsReleased = r.IsReleased })
                    .ToList(),
                Environments = software.Environments
                    .OrderBy(e => e.Name)
                    .Select(e => new EnvironmentDto { Id = e.Id, Name = e.Name })
                    .ToList(),
                Developers = participatingDevs
            };
        }

        public async Task<ComponentDto> AddComponentAsync(Guid softwareId, CreateNamedItemDto dto, Guid currentUserId)
        {
            var software = await ValidateOwnerAccessAsync(softwareId, currentUserId);

            var name = dto.Name.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Название компонента обязательно.");
            }

            if (name.Length > 100)
            {
                throw new ArgumentException("Название компонента не может превышать 100 символов.");
            }

            if (software.Components.Any(c => string.Equals(c.Name, name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException("Компонент с таким названием уже существует в этом ПО.");
            }

            var component = new Component
            {
                Id = Guid.NewGuid(),
                SoftwareId = software.Id,
                Name = name
            };

            software.Components.Add(component);
            await _repository.SaveChangesAsync();

            return new ComponentDto { Id = component.Id, Name = component.Name };
        }

        public async Task<ReleaseDto> AddReleaseAsync(Guid softwareId, CreateReleaseDto dto, Guid currentUserId)
        {
            var software = await ValidateOwnerAccessAsync(softwareId, currentUserId);

            var version = dto.VersionTag.Trim();
            if (string.IsNullOrWhiteSpace(version))
            {
                throw new ArgumentException("Тег версии обязателен.");
            }

            if (version.Length > 100)
            {
                throw new ArgumentException("Тег версии не может превышать 100 символов.");
            }

            if (software.Releases.Any(r => string.Equals(r.VersionTag, version, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException("Релиз с такой версией уже существует в этом ПО.");
            }

            var release = new Release
            {
                Id = Guid.NewGuid(),
                SoftwareId = software.Id,
                VersionTag = version,
                IsReleased = false
            };

            software.Releases.Add(release);
            await _repository.SaveChangesAsync();

            return new ReleaseDto { Id = release.Id, VersionTag = release.VersionTag, IsReleased = release.IsReleased };
        }

        public async Task<EnvironmentDto> AddEnvironmentAsync(Guid softwareId, CreateNamedItemDto dto, Guid currentUserId)
        {
            var software = await ValidateOwnerAccessAsync(softwareId, currentUserId);

            var name = dto.Name.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Название окружения обязательно.");
            }

            if (name.Length > 100)
            {
                throw new ArgumentException("Название окружения не может превышать 100 символов.");
            }

            if (software.Environments.Any(e => string.Equals(e.Name, name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException("Окружение с таким названием уже существует в этом ПО.");
            }

            var environment = new Environment
            {
                Id = Guid.NewGuid(),
                SoftwareId = software.Id,
                Name = name
            };

            software.Environments.Add(environment);
            await _repository.SaveChangesAsync();

            return new EnvironmentDto { Id = environment.Id, Name = environment.Name };
        }

        public async Task<ReleaseDto> ToggleReleaseStatusAsync(Guid softwareId, Guid releaseId, Guid currentUserId)
        {
            var software = await ValidateOwnerAccessAsync(softwareId, currentUserId);

            var release = software.Releases.FirstOrDefault(r => r.Id == releaseId);
            if (release == null)
            {
                throw new KeyNotFoundException("Релиз не найден.");
            }

            release.IsReleased = !release.IsReleased;
            await _repository.SaveChangesAsync();

            return new ReleaseDto { Id = release.Id, VersionTag = release.VersionTag, IsReleased = release.IsReleased };
        }

        private async Task<Software> ValidateOwnerAccessAsync(Guid softwareId, Guid currentUserId)
        {
            var software = await _repository.GetByIdWithDetailsAsync(softwareId);
            if (software == null)
            {
                throw new KeyNotFoundException("Программное обеспечение не найдено.");
            }

            var member = await _repository.GetTeamMemberByIdAsync(currentUserId);
            if (member == null || !member.IsActive)
            {
                throw new UnauthorizedAccessException("Пользователь не найден или деактивирован.");
            }

            if (software.OwnerId != currentUserId)
            {
                throw new UnauthorizedAccessException("Только владелец программного обеспечения может изменять его конфигурацию.");
            }

            return software;
        }

        private static List<string> Normalize(IEnumerable<string>? items, int maxLength, string fieldName)
        {
            var list = (items ?? Enumerable.Empty<string>())
                .Select(x => x?.Trim() ?? string.Empty)
                .Where(x => x.Length > 0)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            foreach (var item in list)
            {
                if (item.Length > maxLength)
                {
                    throw new ArgumentException($"Элемент '{item}' в поле '{fieldName}' превышает допустимую длину в {maxLength} символов.");
                }
            }

            return list;
        }
    }
}
