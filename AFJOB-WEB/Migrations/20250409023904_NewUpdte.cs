using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AFJOB_WEB.Migrations
{
    /// <inheritdoc />
    public partial class NewUpdte : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfileImagePath",
                table: "JobSeekerProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Visibility",
                table: "JobSeekerProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileImagePath",
                table: "JobSeekerProfiles");

            migrationBuilder.DropColumn(
                name: "Visibility",
                table: "JobSeekerProfiles");
        }
    }
}
