namespace server.Models
{
    public class Software
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;
        public string TechStack { get; set; } = string.Empty;

        public Guid OwnerId { get; set; }
        public Tester? Owner { get; set; }

        public ICollection<Component> Components { get; set; } = new List<Component>();
        public ICollection<Release> Releases { get; set; } = new List<Release>();
        public ICollection<Environment> Environments { get; set; } = new List<Environment>();
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
        public ICollection<TeamMember> Members { get; set; } = new List<TeamMember>();
    }

    public class Component
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid SoftwareId { get; set; }
        public Software? Software { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class Release
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid SoftwareId { get; set; }
        public Software? Software { get; set; }
        public string VersionTag { get; set; } = string.Empty;
        public bool IsReleased { get; set; }
    }

    public class Environment
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid SoftwareId { get; set; }
        public Software? Software { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
