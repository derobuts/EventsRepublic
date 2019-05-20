using Microsoft.EntityFrameworkCore.Migrations;

namespace EventsRepublic.Migrations
{
    public partial class changednames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserIntid",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserIntid",
                table: "AspNetUsers");
        }
    }
}
