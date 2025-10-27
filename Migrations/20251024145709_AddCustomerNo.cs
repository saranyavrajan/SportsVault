using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportsVault.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerNo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerNo",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CustomerNo",
                table: "Users",
                column: "CustomerNo",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_CustomerNo",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CustomerNo",
                table: "Users");
        }
    }
}
