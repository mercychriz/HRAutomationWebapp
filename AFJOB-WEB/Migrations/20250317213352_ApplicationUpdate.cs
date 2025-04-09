using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AFJOB_WEB.Migrations
{
    public partial class ApplicationUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppliedAt",
                table: "ApplicationTables");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ApplicationTables",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "ApplicationTables",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "JobId",
                table: "ApplicationTables",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ApplicationDate",
                table: "ApplicationTables",
                type: "datetime2",
                nullable: false,
                defaultValue: DateTime.UtcNow);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationTables_JobId",
                table: "ApplicationTables",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationTables_UserId",
                table: "ApplicationTables",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationTables_Jobs_JobId",
                table: "ApplicationTables",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "JobId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationTables_User_UserId",
                table: "ApplicationTables",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationTables_Jobs_JobId",
                table: "ApplicationTables");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationTables_User_UserId",
                table: "ApplicationTables");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationTables_JobId",
                table: "ApplicationTables");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationTables_UserId",
                table: "ApplicationTables");

            migrationBuilder.DropColumn(
                name: "ApplicationDate",
                table: "ApplicationTables");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "ApplicationTables",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "ApplicationTables",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "JobId",
                table: "ApplicationTables",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "AppliedAt",
                table: "ApplicationTables",
                type: "datetime2",
                nullable: true);
        }
    }
}
