using Microsoft.EntityFrameworkCore;
using server.Models;
using Environment = server.Models.Environment;

namespace server.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<Tester> Testers { get; set; }
        public DbSet<Developer> Developers { get; set; }
        public DbSet<Software> Softwares { get; set; }
        public DbSet<Component> Components { get; set; }
        public DbSet<Environment> Environments { get; set; }
        public DbSet<Release> Releases { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Fix> Fixes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<TicketHistory> TicketHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Administrator>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.HasIndex(a => a.Login).IsUnique();
                entity.Property(a => a.Login).HasMaxLength(100).IsRequired();
                entity.Property(a => a.Email).HasMaxLength(200);
            });

            modelBuilder.Entity<TeamMember>(entity =>
            {
                entity.HasKey(m => m.Id);
                entity.HasIndex(m => m.Login).IsUnique();
                entity.HasIndex(m => m.Email);
                entity.Property(m => m.Login).HasMaxLength(100).IsRequired();
                entity.Property(m => m.FullName).HasMaxLength(200).IsRequired();
                entity.Property(m => m.Email).HasMaxLength(200);
                entity.Property(m => m.Position).HasMaxLength(150);

                entity.HasDiscriminator<string>("Role")
                    .HasValue<Tester>("Tester")
                    .HasValue<Developer>("Developer");

                entity.HasOne(m => m.RegisteredBy)
                    .WithMany(a => a.RegisteredUsers)
                    .HasForeignKey(m => m.RegisteredById)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Software>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Name).HasMaxLength(200).IsRequired();
                entity.Property(s => s.ContactEmail).HasMaxLength(200);
                entity.Property(s => s.TechStack).HasMaxLength(500);

                entity.HasOne(s => s.Owner)
                    .WithMany(t => t.OwnedSoftwares)
                    .HasForeignKey(s => s.OwnerId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(s => s.Members)
                    .WithMany(m => m.SoftwareList);
            });

            modelBuilder.Entity<Component>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Name).HasMaxLength(150).IsRequired();

                entity.HasOne(c => c.Software)
                    .WithMany(s => s.Components)
                    .HasForeignKey(c => c.SoftwareId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Environment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();

                entity.HasOne(e => e.Software)
                    .WithMany(s => s.Environments)
                    .HasForeignKey(e => e.SoftwareId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Release>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.VersionTag).HasMaxLength(100).IsRequired();

                entity.HasOne(r => r.Software)
                    .WithMany(s => s.Releases)
                    .HasForeignKey(r => r.SoftwareId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Title).HasMaxLength(300).IsRequired();
                entity.Property(t => t.Status).HasConversion<string>();
                entity.Property(t => t.Priority).HasConversion<string>();
                entity.Property(t => t.RejectionReason).HasConversion<string>();

                entity.HasOne(t => t.Software)
                    .WithMany(s => s.Tickets)
                    .HasForeignKey(t => t.SoftwareId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(t => t.Component)
                    .WithMany()
                    .HasForeignKey(t => t.ComponentId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.Environment)
                    .WithMany()
                    .HasForeignKey(t => t.EnvironmentId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.FoundInRelease)
                    .WithMany()
                    .HasForeignKey(t => t.FoundInReleaseId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.FixedInRelease)
                    .WithMany()
                    .HasForeignKey(t => t.FixedInReleaseId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(t => t.Reporter)
                    .WithMany(tester => tester.CreatedTickets)
                    .HasForeignKey(t => t.ReporterId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.Assignee)
                    .WithMany(dev => dev.AssignedTickets)
                    .HasForeignKey(t => t.AssigneeId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(t => t.DuplicateOf)
                    .WithMany()
                    .HasForeignKey(t => t.DuplicateOfId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Fix>(entity =>
            {
                entity.HasKey(f => f.Id);
                entity.Property(f => f.MrCommitUrl).HasMaxLength(1000).IsRequired();

                entity.HasOne(f => f.Ticket)
                    .WithMany(t => t.Fixes)
                    .HasForeignKey(f => f.TicketId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(f => f.Developer)
                    .WithMany(d => d.SubmittedFixes)
                    .HasForeignKey(f => f.DeveloperId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(f => f.VerifiedOn)
                    .WithMany()
                    .HasForeignKey(f => f.VerifiedOnEnvId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Text).IsRequired();

                entity.HasOne(c => c.Ticket)
                    .WithMany(t => t.Comments)
                    .HasForeignKey(c => c.TicketId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(c => c.Author)
                    .WithMany(a => a.Comments)
                    .HasForeignKey(c => c.AuthorId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Attachment>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a => a.FileName).HasMaxLength(300).IsRequired();
                entity.Property(a => a.StorageUrl).HasMaxLength(1500).IsRequired();

                entity.HasOne(a => a.Ticket)
                    .WithMany(t => t.Attachments)
                    .HasForeignKey(a => a.TicketId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(a => a.UploadedBy)
                    .WithMany()
                    .HasForeignKey(a => a.UploadedById)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<TicketHistory>(entity =>
            {
                entity.HasKey(h => h.Id);
                entity.Property(h => h.FieldChanged).HasMaxLength(100).IsRequired();
                entity.Property(h => h.RejectionReason).HasConversion<string>();

                entity.HasOne(h => h.Ticket)
                    .WithMany(t => t.History)
                    .HasForeignKey(h => h.TicketId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(h => h.ChangedBy)
                    .WithMany()
                    .HasForeignKey(h => h.ChangedById)
                    .OnDelete(DeleteBehavior.SetNull);
            });
        }
    }
}
