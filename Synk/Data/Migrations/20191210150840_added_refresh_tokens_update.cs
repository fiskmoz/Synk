using Microsoft.EntityFrameworkCore.Migrations;

namespace Synk.Data.Migrations
{
    public partial class added_refresh_tokens_update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "invalidated",
                table: "RefreshTokens",
                newName: "Invalidated");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Invalidated",
                table: "RefreshTokens",
                newName: "invalidated");
        }
    }
}
