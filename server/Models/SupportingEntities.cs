using server.Models.Enums;

namespace server.Models
{
    public class Fix
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid TicketId { get; set; }
        public Ticket? Ticket { get; set; }

        public Guid DeveloperId { get; set; }
        public Developer? Developer { get; set; }

        public string MrCommitUrl { get; set; } = string.Empty;

        public Guid VerifiedOnEnvId { get; set; }
        public Environment? VerifiedOn { get; set; }

        public string? CommentText { get; set; }
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    }

    public class Comment
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid TicketId { get; set; }
        public Ticket? Ticket { get; set; }

        public Guid AuthorId { get; set; }
        public TeamMember? Author { get; set; }

        public string Text { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public string ReactionsJson { get; set; } = "{}";
    }

    public class Attachment
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid TicketId { get; set; }
        public Ticket? Ticket { get; set; }

        public string FileName { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public string StorageUrl { get; set; } = string.Empty;

        public Guid? UploadedById { get; set; }
        public TeamMember? UploadedBy { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }

    public class TicketHistory
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid TicketId { get; set; }
        public Ticket? Ticket { get; set; }

        public Guid? ChangedById { get; set; }
        public TeamMember? ChangedBy { get; set; }

        public string FieldChanged { get; set; } = string.Empty;
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public RejectionReason? RejectionReason { get; set; }
        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
    }
}
