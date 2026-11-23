namespace server.Models
{
    public abstract class TeamMember
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FullName { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        public Guid RegisteredById { get; set; }
        public Administrator RegisteredBy { get; set; } = null!;

        public ICollection<Software> ParticipatingSoftwares { get; set; } = new List<Software>();
    }

    public class Tester : TeamMember
    {
        public ICollection<Ticket> CreatedTickets { get; set; } = new List<Ticket>();
        public ICollection<Software> OwnedSoftwares { get; set; } = new List<Software>();
    }

    public class Developer : TeamMember
    {
        public ICollection<Ticket> AssignedTickets { get; set; } = new List<Ticket>();
        public ICollection<Fix> SubmittedFixes { get; set; } = new List<Fix>();
    }
}
