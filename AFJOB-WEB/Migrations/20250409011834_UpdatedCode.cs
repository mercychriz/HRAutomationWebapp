using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AFJOB_WEB.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ResumeFilePath",
                table: "JobSeekerProfiles",
                newName: "ResumeFile");

            migrationBuilder.AddColumn<string>(
                name: "Visibility",
                table: "Jobs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsSelected",
                table: "ApplicationTables",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "OfferDate",
                table: "ApplicationTables",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OfferDetails",
                table: "ApplicationTables",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Visibility",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "IsSelected",
                table: "ApplicationTables");

            migrationBuilder.DropColumn(
                name: "OfferDate",
                table: "ApplicationTables");

            migrationBuilder.DropColumn(
                name: "OfferDetails",
                table: "ApplicationTables");

            migrationBuilder.RenameColumn(
                name: "ResumeFile",
                table: "JobSeekerProfiles",
                newName: "ResumeFilePath");
        }
    }
}
