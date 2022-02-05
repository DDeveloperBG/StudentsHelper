using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentsHelper.Data.Migrations
{
    public partial class AddedIsPaidToTeacherToStudentsTransactions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StudentsTransactions_ConsultationId",
                table: "StudentsTransactions");

            migrationBuilder.AddColumn<bool>(
                name: "IsPaidToTeacher",
                table: "StudentsTransactions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_StudentsTransactions_ConsultationId",
                table: "StudentsTransactions",
                column: "ConsultationId",
                unique: true,
                filter: "[ConsultationId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StudentsTransactions_ConsultationId",
                table: "StudentsTransactions");

            migrationBuilder.DropColumn(
                name: "IsPaidToTeacher",
                table: "StudentsTransactions");

            migrationBuilder.CreateIndex(
                name: "IX_StudentsTransactions_ConsultationId",
                table: "StudentsTransactions",
                column: "ConsultationId");
        }
    }
}
