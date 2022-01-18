using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentsHelper.Data.Migrations
{
    public partial class AddedToStudentTransactionEntityTeacherRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ToTeacherId",
                table: "StudentsTransactions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentsTransactions_ToTeacherId",
                table: "StudentsTransactions",
                column: "ToTeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentsTransactions_Teachers_ToTeacherId",
                table: "StudentsTransactions",
                column: "ToTeacherId",
                principalTable: "Teachers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentsTransactions_Teachers_ToTeacherId",
                table: "StudentsTransactions");

            migrationBuilder.DropIndex(
                name: "IX_StudentsTransactions_ToTeacherId",
                table: "StudentsTransactions");

            migrationBuilder.DropColumn(
                name: "ToTeacherId",
                table: "StudentsTransactions");
        }
    }
}
