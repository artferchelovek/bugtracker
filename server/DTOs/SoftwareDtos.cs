using System.ComponentModel.DataAnnotations;

namespace server.DTOs
{
    public class CreateSoftwareDto
    {
        [Required(ErrorMessage = "Название программного обеспечения обязательно.")]
        [MaxLength(200, ErrorMessage = "Название не может превышать 200 символов.")]
        public string Name { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Некорректный формат контактного email.")]
        [MaxLength(200, ErrorMessage = "Email не может превышать 200 символов.")]
        public string? ContactEmail { get; set; }

        [MaxLength(500, ErrorMessage = "Стек технологий не может превышать 500 символов.")]
        public string? TechStack { get; set; }

        public List<string>? Components { get; set; }
        public List<string>? Releases { get; set; }
        public List<string>? Environments { get; set; }
        public List<Guid>? DeveloperIds { get; set; }
    }

    public class CreateNamedItemDto
    {
        [Required(ErrorMessage = "Название обязательно.")]
        [MaxLength(150, ErrorMessage = "Название не может превышать 150 символов.")]
        public string Name { get; set; } = string.Empty;
    }

    public class CreateReleaseDto
    {
        [Required(ErrorMessage = "Тег версии обязателен.")]
        [MaxLength(100, ErrorMessage = "Тег версии не может превышать 100 символов.")]
        public string VersionTag { get; set; } = string.Empty;
    }

    public class SoftwareDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ContactEmail { get; set; }
        public string? TechStack { get; set; }
        public Guid OwnerId { get; set; }
        public string OwnerName { get; set; } = string.Empty;
        public bool IsInMyList { get; set; }
        public int ComponentsCount { get; set; }
        public int ReleasesCount { get; set; }
        public int EnvironmentsCount { get; set; }
        public int OpenTicketsCount { get; set; }
    }

    public class ComponentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class ReleaseDto
    {
        public Guid Id { get; set; }
        public string VersionTag { get; set; } = string.Empty;
        public bool IsReleased { get; set; }
    }

    public class EnvironmentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class SoftwareParamsDto
    {
        public Guid SoftwareId { get; set; }
        public string SoftwareName { get; set; } = string.Empty;
        public List<ComponentDto> Components { get; set; } = new();
        public List<ReleaseDto> Releases { get; set; } = new();
        public List<EnvironmentDto> Environments { get; set; } = new();
        public List<UserDto> Developers { get; set; } = new();
    }
}
