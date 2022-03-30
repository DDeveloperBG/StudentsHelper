using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentsHelper.Data.Migrations
{
    public partial class AddedStudentFavouriteTeacherEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StudentsFavouriteTeachers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StillIsFavourite = table.Column<bool>(type: "bit", nullable: false),
                    StudentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TeacherId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentsFavouriteTeachers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentsFavouriteTeachers_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StudentsFavouriteTeachers_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentsFavouriteTeachers_StudentId",
                table: "StudentsFavouriteTeachers",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentsFavouriteTeachers_TeacherId",
                table: "StudentsFavouriteTeachers",
                column: "TeacherId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentsFavouriteTeachers");
        }
    }
}
