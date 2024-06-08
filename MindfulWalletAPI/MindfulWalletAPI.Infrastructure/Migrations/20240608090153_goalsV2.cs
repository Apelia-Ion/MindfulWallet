using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MindfulWallet.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class goalsV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExpenseId",
                table: "Events",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpenseId",
                table: "Events");
        }
    }
}
