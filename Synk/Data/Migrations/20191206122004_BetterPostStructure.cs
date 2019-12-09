using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Synk.Data.Migrations
{
    public partial class BetterPostStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PublishDate = table.Column<DateTime>(nullable: false),
                    Body = table.Column<string>(nullable: true),
                    Author = table.Column<string>(nullable: true),
                    Likes = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Posts");
        }
    }
}
