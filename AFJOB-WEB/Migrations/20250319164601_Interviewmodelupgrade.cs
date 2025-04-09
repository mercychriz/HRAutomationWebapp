using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AFJOB_WEB.Migrations
{
    /// <inheritdoc />
    public partial class Interviewmodelupgrade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
     name: "JobSeekerProfileId",
     table: "ApplicationTables",
     type: "int",
     nullable: true);  // ✅ allow nulls initially!


            migrationBuilder.CreateTable(
                name: "JobSeekerProfiles",
                columns: table => new
                {
                    JobSeekerProfileId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResumeFilePath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobSeekerProfiles", x => x.JobSeekerProfileId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationTables_JobSeekerProfileId",
                table: "ApplicationTables",
                column: "JobSeekerProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationTables_JobSeekerProfiles_JobSeekerProfileId",
                table: "ApplicationTables",
                column: "JobSeekerProfileId",
                principalTable: "JobSeekerProfiles",
                principalColumn: "JobSeekerProfileId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationTables_JobSeekerProfiles_JobSeekerProfileId",
                table: "ApplicationTables");

            migrationBuilder.DropTable(
                name: "JobSeekerProfiles");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationTables_JobSeekerProfileId",
                table: "ApplicationTables");

            migrationBuilder.DropColumn(
                name: "JobSeekerProfileId",
                table: "ApplicationTables");
        }
    }
}
