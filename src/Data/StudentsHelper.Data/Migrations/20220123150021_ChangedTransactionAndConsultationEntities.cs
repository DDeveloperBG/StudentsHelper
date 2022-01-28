using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentsHelper.Data.Migrations
{
    public partial class ChangedTransactionAndConsultationEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentsTransactions_Students_StudentId",
                table: "StudentsTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentsTransactions_Teachers_ToTeacherId",
                table: "StudentsTransactions");

            migrationBuilder.RenameColumn(
                name: "ToTeacherId",
                table: "StudentsTransactions",
                newName: "TeacherId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentsTransactions_ToTeacherId",
                table: "StudentsTransactions",
                newName: "IX_StudentsTransactions_TeacherId");

            migrationBuilder.AlterColumn<string>(
                name: "StudentId",
                table: "StudentsTransactions",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "ConsultationId",
                table: "StudentsTransactions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Consultation",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HourWage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StudentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TeacherId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consultation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Consultation_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Consultation_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentsTransactions_ConsultationId",
                table: "StudentsTransactions",
                column: "ConsultationId");

            migrationBuilder.CreateIndex(
                name: "IX_Consultation_IsDeleted",
                table: "Consultation",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Consultation_StudentId",
                table: "Consultation",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Consultation_TeacherId",
                table: "Consultation",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentsTransactions_Consultation_ConsultationId",
                table: "StudentsTransactions",
                column: "ConsultationId",
                principalTable: "Consultation",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentsTransactions_Students_StudentId",
                table: "StudentsTransactions",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentsTransactions_Teachers_TeacherId",
                table: "StudentsTransactions",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentsTransactions_Consultation_ConsultationId",
                table: "StudentsTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentsTransactions_Students_StudentId",
                table: "StudentsTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentsTransactions_Teachers_TeacherId",
                table: "StudentsTransactions");

            migrationBuilder.DropTable(
                name: "Consultation");

            migrationBuilder.DropIndex(
                name: "IX_StudentsTransactions_ConsultationId",
                table: "StudentsTransactions");

            migrationBuilder.DropColumn(
                name: "ConsultationId",
                table: "StudentsTransactions");

            migrationBuilder.RenameColumn(
                name: "TeacherId",
                table: "StudentsTransactions",
                newName: "ToTeacherId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentsTransactions_TeacherId",
                table: "StudentsTransactions",
                newName: "IX_StudentsTransactions_ToTeacherId");

            migrationBuilder.AlterColumn<string>(
                name: "StudentId",
                table: "StudentsTransactions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentsTransactions_Students_StudentId",
                table: "StudentsTransactions",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentsTransactions_Teachers_ToTeacherId",
                table: "StudentsTransactions",
                column: "ToTeacherId",
                principalTable: "Teachers",
                principalColumn: "Id");
        }
    }
}
