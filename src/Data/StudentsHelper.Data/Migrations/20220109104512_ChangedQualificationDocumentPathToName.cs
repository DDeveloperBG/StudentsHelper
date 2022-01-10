#nullable disable

namespace StudentsHelper.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class ChangedQualificationDocumentPathToName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QualificationDocumentPath",
                table: "Teachers",
                newName: "QualificationDocumentName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QualificationDocumentName",
                table: "Teachers",
                newName: "QualificationDocumentPath");
        }
    }
}
