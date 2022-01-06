using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentsHelper.Data.Migrations
{
    public partial class AddedIsRejectedColumnToTeachers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRejected",
                table: "Teachers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRejected",
                table: "Teachers");
        }
    }
}
