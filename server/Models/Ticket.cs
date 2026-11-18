using server.Models.Enums;

namespace server.Models
{
    public class Ticket
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string StepsToReproduce { get; set; } = string.Empty;
        public TicketStatus Status { get; set; } = TicketStatus.OPEN;
        public Priority Priority { get; set; } = Priority.P3_MEDIUM;
        public DateTime? Deadline { get; set; }
        public RejectionReason? RejectionReason { get; set; }
        public string? StatusReasonText { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public Guid SoftwareId { get; set; }
        public Software? Software { get; set; }

        public Guid ComponentId { get; set; }
        public Component? Component { get; set; }

        public Guid EnvironmentId { get; set; }
        public Environment? Environment { get; set; }

        public Guid FoundInReleaseId { get; set; }
        public Release? FoundInRelease { get; set; }

        public Guid? FixedInReleaseId { get; set; }
        public Release? FixedInRelease { get; set; }

        public Guid ReporterId { get; set; }
        public Tester? Reporter { get; set; }

        public Guid? AssigneeId { get; set; }
        public Developer? Assignee { get; set; }

        public Guid? DuplicateOfId { get; set; }
        public Ticket? DuplicateOf { get; set; }

        public ICollection<Fix> Fixes { get; set; } = new List<Fix>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
        public ICollection<TicketHistory> History { get; set; } = new List<TicketHistory>();
    }
}
