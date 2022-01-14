using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentsHelper.Data.Migrations
{
    public partial class ChangedTeacherQDNameToPath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QualificationDocumentName",
                table: "Teachers",
                newName: "QualificationDocumentPath");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QualificationDocumentPath",
                table: "Teachers",
                newName: "QualificationDocumentName");
        }
    }
}
