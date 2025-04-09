using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AFJOB_WEB.Migrations
{
    /// <inheritdoc />
    public partial class Interviewupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Interviews_Candidates_CandidateId",
                table: "Interviews");

            migrationBuilder.RenameColumn(
                name: "CandidateId",
                table: "Interviews",
                newName: "ApplicationId");

            migrationBuilder.RenameIndex(
                name: "IX_Interviews_CandidateId",
                table: "Interviews",
                newName: "IX_Interviews_ApplicationId");

            migrationBuilder.AddColumn<string>(
                name: "InterviewStatus",
                table: "Interviews",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Interviews_ApplicationTables_ApplicationId",
                table: "Interviews",
                column: "ApplicationId",
                principalTable: "ApplicationTables",
                principalColumn: "ApplicationId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Interviews_ApplicationTables_ApplicationId",
                table: "Interviews");

            migrationBuilder.DropColumn(
                name: "InterviewStatus",
                table: "Interviews");

            migrationBuilder.RenameColumn(
                name: "ApplicationId",
                table: "Interviews",
                newName: "CandidateId");

            migrationBuilder.RenameIndex(
                name: "IX_Interviews_ApplicationId",
                table: "Interviews",
                newName: "IX_Interviews_CandidateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Interviews_Candidates_CandidateId",
                table: "Interviews",
                column: "CandidateId",
                principalTable: "Candidates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
