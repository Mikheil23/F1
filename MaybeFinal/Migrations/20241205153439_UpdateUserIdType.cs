using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaybeFinal.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserIdType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop the foreign key constraints first
            migrationBuilder.DropForeignKey(
       name: "FK_Loans_Users_UserId",
       table: "Loans");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Accountants_AccountantId",
                table: "Users");

            // Drop the index that depends on UserId
            migrationBuilder.DropIndex(
                name: "IX_Loans_UserId",
                table: "Loans");

            // Drop the UserId column from the Loans table
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Loans");

            // Recreate the UserId column as nvarchar(450) (without Identity)
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Loans",
                type: "nvarchar(450)",
                nullable: false);

            // Add the foreign key constraint again, linking UserId to Users table
            migrationBuilder.AddForeignKey(
                name: "FK_Loans_Users_UserId",
                table: "Loans",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            // Recreate the index for UserId in Loans table
            migrationBuilder.CreateIndex(
                name: "IX_Loans_UserId",
                table: "Loans",
                column: "UserId");

            // Add the foreign key constraint to Users table
            migrationBuilder.AddForeignKey(
                name: "FK_Users_Accountants_AccountantId",
                table: "Users",
                column: "AccountantId",
                principalTable: "Accountants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Reverse the changes by dropping the index and column, and restoring the previous state
            migrationBuilder.DropForeignKey(
                name: "FK_Loans_Users_UserId",
                table: "Loans");

            migrationBuilder.DropIndex(
                name: "IX_Loans_UserId",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Loans");

            // Add the old UserId column back as string
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Loans",
                type: "nvarchar(450)",
                nullable: false);

            // Recreate the foreign key relationship for the old column type
            migrationBuilder.AddForeignKey(
                name: "FK_Loans_Users_UserId",
                table: "Loans",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            // Recreate the index for UserId column
            migrationBuilder.CreateIndex(
                name: "IX_Loans_UserId",
                table: "Loans",
                column: "UserId");

            // Drop foreign key for Users -> Accountants relationship
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Accountants_AccountantId",
                table: "Users");
        }
    }





}
