#nullable disable

namespace StudentsHelper.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddedMeetingHasStartedProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasStarted",
                table: "Meetings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasStarted",
                table: "Meetings");
        }
    }
}
