using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Data.Migrations
{
    public partial class CaseInsensitiveIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Softwares_Name",
                table: "Softwares");

            migrationBuilder.DropIndex(
                name: "IX_Releases_SoftwareId_VersionTag",
                table: "Releases");

            migrationBuilder.DropIndex(
                name: "IX_Environments_SoftwareId_Name",
                table: "Environments");

            migrationBuilder.DropIndex(
                name: "IX_Components_SoftwareId_Name",
                table: "Components");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Components",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150);

            migrationBuilder.CreateIndex(
                name: "IX_Releases_SoftwareId",
                table: "Releases",
                column: "SoftwareId");

            migrationBuilder.CreateIndex(
                name: "IX_Environments_SoftwareId",
                table: "Environments",
                column: "SoftwareId");

            migrationBuilder.CreateIndex(
                name: "IX_Components_SoftwareId",
                table: "Components",
                column: "SoftwareId");

            migrationBuilder.Sql(@"CREATE UNIQUE INDEX ""IX_Softwares_Name_Lower"" ON ""Softwares"" (LOWER(""Name""));");
            migrationBuilder.Sql(@"CREATE UNIQUE INDEX ""IX_Components_SoftwareId_Name_Lower"" ON ""Components"" (""SoftwareId"", LOWER(""Name""));");
            migrationBuilder.Sql(@"CREATE UNIQUE INDEX ""IX_Environments_SoftwareId_Name_Lower"" ON ""Environments"" (""SoftwareId"", LOWER(""Name""));");
            migrationBuilder.Sql(@"CREATE UNIQUE INDEX ""IX_Releases_SoftwareId_VersionTag_Lower"" ON ""Releases"" (""SoftwareId"", LOWER(""VersionTag""));");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP INDEX IF EXISTS ""IX_Softwares_Name_Lower"";");
            migrationBuilder.Sql(@"DROP INDEX IF EXISTS ""IX_Components_SoftwareId_Name_Lower"";");
            migrationBuilder.Sql(@"DROP INDEX IF EXISTS ""IX_Environments_SoftwareId_Name_Lower"";");
            migrationBuilder.Sql(@"DROP INDEX IF EXISTS ""IX_Releases_SoftwareId_VersionTag_Lower"";");

            migrationBuilder.DropIndex(
                name: "IX_Releases_SoftwareId",
                table: "Releases");

            migrationBuilder.DropIndex(
                name: "IX_Environments_SoftwareId",
                table: "Environments");

            migrationBuilder.DropIndex(
                name: "IX_Components_SoftwareId",
                table: "Components");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Components",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.CreateIndex(
                name: "IX_Softwares_Name",
                table: "Softwares",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Releases_SoftwareId_VersionTag",
                table: "Releases",
                columns: new[] { "SoftwareId", "VersionTag" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Environments_SoftwareId_Name",
                table: "Environments",
                columns: new[] { "SoftwareId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Components_SoftwareId_Name",
                table: "Components",
                columns: new[] { "SoftwareId", "Name" },
                unique: true);
        }
    }
}
