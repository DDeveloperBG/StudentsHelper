using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentsHelper.Data.Migrations
{
    public partial class AddedSchoolSubjectToConsultation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SchoolSubjectId",
                table: "Consultation",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Consultation_SchoolSubjectId",
                table: "Consultation",
                column: "SchoolSubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Consultation_Subjects_SchoolSubjectId",
                table: "Consultation",
                column: "SchoolSubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consultation_Subjects_SchoolSubjectId",
                table: "Consultation");

            migrationBuilder.DropIndex(
                name: "IX_Consultation_SchoolSubjectId",
                table: "Consultation");

            migrationBuilder.DropColumn(
                name: "SchoolSubjectId",
                table: "Consultation");
        }
    }
}
