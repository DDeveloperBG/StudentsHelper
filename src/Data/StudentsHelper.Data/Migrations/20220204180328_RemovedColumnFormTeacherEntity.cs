#nullable disable

namespace StudentsHelper.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class RemovedColumnFormTeacherEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsExpressConnectedAccountConfirmed",
                table: "Teachers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsExpressConnectedAccountConfirmed",
                table: "Teachers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
