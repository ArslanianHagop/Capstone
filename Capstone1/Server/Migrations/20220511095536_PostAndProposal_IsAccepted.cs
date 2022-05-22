using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Capstone1.Server.Migrations
{
    public partial class PostAndProposal_IsAccepted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAccepted",
                table: "Proposals",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAccepted",
                table: "Posts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAccepted",
                table: "Proposals");

            migrationBuilder.DropColumn(
                name: "IsAccepted",
                table: "Posts");
        }
    }
}
