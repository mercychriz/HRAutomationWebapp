using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AFJOB_WEB.Migrations
{
    /// <inheritdoc />
    public partial class Applicati : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop existing foreign key if already created with NOT NULL
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationTables_JobSeekerProfiles_JobSeekerProfileId",
                table: "ApplicationTables");

            // Make JobSeekerProfileId nullable
            migrationBuilder.AlterColumn<int>(
                name: "JobSeekerProfileId",
                table: "ApplicationTables",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            // Recreate Foreign Key with updated nullable relationship
            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationTables_JobSeekerProfiles_JobSeekerProfileId",
                table: "ApplicationTables",
                column: "JobSeekerProfileId",
                principalTable: "JobSeekerProfiles",
                principalColumn: "JobSeekerProfileId",
                onDelete: ReferentialAction.SetNull); // ✅ Safer option than Cascade
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop updated foreign key
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationTables_JobSeekerProfiles_JobSeekerProfileId",
                table: "ApplicationTables");

            // Make JobSeekerProfileId NOT NULL again if rolling back
            migrationBuilder.AlterColumn<int>(
                name: "JobSeekerProfileId",
                table: "ApplicationTables",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            // Recreate original foreign key
            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationTables_JobSeekerProfiles_JobSeekerProfileId",
                table: "ApplicationTables",
                column: "JobSeekerProfileId",
                principalTable: "JobSeekerProfiles",
                principalColumn: "JobSeekerProfileId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
