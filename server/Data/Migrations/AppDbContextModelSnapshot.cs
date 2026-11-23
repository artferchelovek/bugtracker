using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using server.Data;

#nullable disable

namespace server.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "10.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("SoftwareTeamMember", b =>
                {
                    b.Property<Guid>("MembersId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ParticipatingSoftwaresId")
                        .HasColumnType("uuid");

                    b.HasKey("MembersId", "ParticipatingSoftwaresId");

                    b.HasIndex("ParticipatingSoftwaresId");

                    b.ToTable("SoftwareTeamMember");
                });

            modelBuilder.Entity("server.Models.Administrator", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Login")
                        .IsUnique();

                    b.ToTable("Administrators");
                });

            modelBuilder.Entity("server.Models.Attachment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("character varying(300)");

                    b.Property<long>("FileSize")
                        .HasColumnType("bigint");

                    b.Property<string>("StorageUrl")
                        .IsRequired()
                        .HasMaxLength(1500)
                        .HasColumnType("character varying(1500)");

                    b.Property<Guid>("TicketId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("UploadedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("UploadedById")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("TicketId");

                    b.HasIndex("UploadedById");

                    b.ToTable("Attachments");
                });

            modelBuilder.Entity("server.Models.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Reactions")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("TicketId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("TicketId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("server.Models.Component", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<Guid>("SoftwareId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("SoftwareId", "Name")
                        .IsUnique();

                    b.ToTable("Components");
                });

            modelBuilder.Entity("server.Models.Environment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<Guid>("SoftwareId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("SoftwareId", "Name")
                        .IsUnique();

                    b.ToTable("Environments");
                });

            modelBuilder.Entity("server.Models.Fix", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CommentText")
                        .HasColumnType("text");

                    b.Property<Guid>("DeveloperId")
                        .HasColumnType("uuid");

                    b.Property<string>("MrCommitUrl")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");

                    b.Property<DateTime>("SubmittedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("TicketId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("VerifiedOnEnvId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("DeveloperId");

                    b.HasIndex("TicketId");

                    b.HasIndex("VerifiedOnEnvId");

                    b.ToTable("Fixes");
                });

            modelBuilder.Entity("server.Models.Release", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("IsReleased")
                        .HasColumnType("boolean");

                    b.Property<Guid>("SoftwareId")
                        .HasColumnType("uuid");

                    b.Property<string>("VersionTag")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.HasIndex("SoftwareId", "VersionTag")
                        .IsUnique();

                    b.ToTable("Releases");
                });

            modelBuilder.Entity("server.Models.Software", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ContactEmail")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid");

                    b.Property<string>("TechStack")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("OwnerId");

                    b.ToTable("Softwares");
                });

            modelBuilder.Entity("server.Models.TeamMember", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<Guid>("RegisteredById")
                        .HasColumnType("uuid");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("character varying(13)");

                    b.HasKey("Id");

                    b.HasIndex("Email");

                    b.HasIndex("Login")
                        .IsUnique();

                    b.HasIndex("RegisteredById");

                    b.ToTable("TeamMembers");

                    b.HasDiscriminator<string>("Role").HasValue("TeamMember");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("server.Models.Ticket", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("AssigneeId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ComponentId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("Deadline")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("DuplicateOfId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("EnvironmentId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("FixedInReleaseId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("FoundInReleaseId")
                        .HasColumnType("uuid");

                    b.Property<string>("Priority")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("RejectionReason")
                        .HasColumnType("text");

                    b.Property<Guid>("ReporterId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SoftwareId")
                        .HasColumnType("uuid");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("StatusReasonText")
                        .HasColumnType("text");

                    b.Property<string>("StepsToReproduce")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("character varying(300)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("AssigneeId");

                    b.HasIndex("ComponentId");

                    b.HasIndex("DuplicateOfId");

                    b.HasIndex("EnvironmentId");

                    b.HasIndex("FixedInReleaseId");

                    b.HasIndex("FoundInReleaseId");

                    b.HasIndex("ReporterId");

                    b.HasIndex("SoftwareId");

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("server.Models.TicketHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("ChangedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("ChangedById")
                        .HasColumnType("uuid");

                    b.Property<string>("FieldChanged")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("NewValue")
                        .HasColumnType("text");

                    b.Property<string>("OldValue")
                        .HasColumnType("text");

                    b.Property<string>("RejectionReason")
                        .HasColumnType("text");

                    b.Property<Guid>("TicketId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ChangedById");

                    b.HasIndex("TicketId");

                    b.ToTable("TicketHistories");
                });

            modelBuilder.Entity("server.Models.Developer", b =>
                {
                    b.HasBaseType("server.Models.TeamMember");

                    b.HasDiscriminator().HasValue("Developer");
                });

            modelBuilder.Entity("server.Models.Tester", b =>
                {
                    b.HasBaseType("server.Models.TeamMember");

                    b.HasDiscriminator().HasValue("Tester");
                });

            modelBuilder.Entity("SoftwareTeamMember", b =>
                {
                    b.HasOne("server.Models.TeamMember", null)
                        .WithMany()
                        .HasForeignKey("MembersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("server.Models.Software", null)
                        .WithMany()
                        .HasForeignKey("ParticipatingSoftwaresId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("server.Models.Attachment", b =>
                {
                    b.HasOne("server.Models.Ticket", "Ticket")
                        .WithMany("Attachments")
                        .HasForeignKey("TicketId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("server.Models.TeamMember", "UploadedBy")
                        .WithMany()
                        .HasForeignKey("UploadedById")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Ticket");

                    b.Navigation("UploadedBy");
                });

            modelBuilder.Entity("server.Models.Comment", b =>
                {
                    b.HasOne("server.Models.TeamMember", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("server.Models.Ticket", "Ticket")
                        .WithMany("Comments")
                        .HasForeignKey("TicketId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("Ticket");
                });

            modelBuilder.Entity("server.Models.Component", b =>
                {
                    b.HasOne("server.Models.Software", "Software")
                        .WithMany("Components")
                        .HasForeignKey("SoftwareId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Software");
                });

            modelBuilder.Entity("server.Models.Environment", b =>
                {
                    b.HasOne("server.Models.Software", "Software")
                        .WithMany("Environments")
                        .HasForeignKey("SoftwareId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Software");
                });

            modelBuilder.Entity("server.Models.Fix", b =>
                {
                    b.HasOne("server.Models.Developer", "Developer")
                        .WithMany("SubmittedFixes")
                        .HasForeignKey("DeveloperId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("server.Models.Ticket", "Ticket")
                        .WithMany("Fixes")
                        .HasForeignKey("TicketId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("server.Models.Environment", "VerifiedOn")
                        .WithMany()
                        .HasForeignKey("VerifiedOnEnvId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Developer");

                    b.Navigation("Ticket");

                    b.Navigation("VerifiedOn");
                });

            modelBuilder.Entity("server.Models.Release", b =>
                {
                    b.HasOne("server.Models.Software", "Software")
                        .WithMany("Releases")
                        .HasForeignKey("SoftwareId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Software");
                });

            modelBuilder.Entity("server.Models.Software", b =>
                {
                    b.HasOne("server.Models.Tester", "Owner")
                        .WithMany("OwnedSoftwares")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("server.Models.TeamMember", b =>
                {
                    b.HasOne("server.Models.Administrator", "RegisteredBy")
                        .WithMany("RegisteredUsers")
                        .HasForeignKey("RegisteredById")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("RegisteredBy");
                });

            modelBuilder.Entity("server.Models.Ticket", b =>
                {
                    b.HasOne("server.Models.Developer", "Assignee")
                        .WithMany("AssignedTickets")
                        .HasForeignKey("AssigneeId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("server.Models.Component", "Component")
                        .WithMany()
                        .HasForeignKey("ComponentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("server.Models.Ticket", "DuplicateOf")
                        .WithMany()
                        .HasForeignKey("DuplicateOfId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("server.Models.Environment", "Environment")
                        .WithMany()
                        .HasForeignKey("EnvironmentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("server.Models.Release", "FixedInRelease")
                        .WithMany()
                        .HasForeignKey("FixedInReleaseId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("server.Models.Release", "FoundInRelease")
                        .WithMany()
                        .HasForeignKey("FoundInReleaseId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("server.Models.Tester", "Reporter")
                        .WithMany("CreatedTickets")
                        .HasForeignKey("ReporterId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("server.Models.Software", "Software")
                        .WithMany("Tickets")
                        .HasForeignKey("SoftwareId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Assignee");

                    b.Navigation("Component");

                    b.Navigation("DuplicateOf");

                    b.Navigation("Environment");

                    b.Navigation("FixedInRelease");

                    b.Navigation("FoundInRelease");

                    b.Navigation("Reporter");

                    b.Navigation("Software");
                });

            modelBuilder.Entity("server.Models.TicketHistory", b =>
                {
                    b.HasOne("server.Models.TeamMember", "ChangedBy")
                        .WithMany()
                        .HasForeignKey("ChangedById")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("server.Models.Ticket", "Ticket")
                        .WithMany("History")
                        .HasForeignKey("TicketId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ChangedBy");

                    b.Navigation("Ticket");
                });

            modelBuilder.Entity("server.Models.Administrator", b =>
                {
                    b.Navigation("RegisteredUsers");
                });

            modelBuilder.Entity("server.Models.Software", b =>
                {
                    b.Navigation("Components");

                    b.Navigation("Environments");

                    b.Navigation("Releases");

                    b.Navigation("Tickets");
                });

            modelBuilder.Entity("server.Models.Ticket", b =>
                {
                    b.Navigation("Attachments");

                    b.Navigation("Comments");

                    b.Navigation("Fixes");

                    b.Navigation("History");
                });

            modelBuilder.Entity("server.Models.Developer", b =>
                {
                    b.Navigation("AssignedTickets");

                    b.Navigation("SubmittedFixes");
                });

            modelBuilder.Entity("server.Models.Tester", b =>
                {
                    b.Navigation("CreatedTickets");

                    b.Navigation("OwnedSoftwares");
                });
#pragma warning restore 612, 618
        }
    }
}
