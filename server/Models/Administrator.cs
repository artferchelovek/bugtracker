namespace server.Models
{
    public class Administrator
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Login { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public ICollection<TeamMember> RegisteredUsers { get; set; } = new List<TeamMember>();
    }
}
