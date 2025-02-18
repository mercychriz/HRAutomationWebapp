using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AFJOB_WEB.Migrations
{
    public partial class SetUserIdAsIdentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop the primary key constraint before dropping the UserID column
            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            // Drop the existing UserID column
            migrationBuilder.DropColumn(
                name: "UserID",
                table: "User");

            // Recreate the UserID column as an identity column
            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "User",
                type: "int",
                nullable: false
            )
            .Annotation("SqlServer:Identity", "1, 1");

            // Re-add the primary key constraint on the UserID column
            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Reverse the changes: Drop the primary key first
            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            // Drop the UserID identity column
            migrationBuilder.DropColumn(
                name: "UserID",
                table: "User");

            // Recreate the UserID column without the identity property
            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // Re-add the primary key constraint for the non-identity column
            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "UserID");
        }
    }
}
