using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentsHelper.Data.Migrations
{
    public partial class AddedSchoolSubjectIconPath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IconPath",
                table: "Subjects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IconPath",
                table: "Subjects");
        }
    }
}
