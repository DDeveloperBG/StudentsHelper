using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentsHelper.Data.Migrations
{
    public partial class ChangedStudentsTransactionsAmountToDecimal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "StudentsTransactions",
                type: "DECIMAL(8,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Amount",
                table: "StudentsTransactions",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "DECIMAL(8,2)");
        }
    }
}
