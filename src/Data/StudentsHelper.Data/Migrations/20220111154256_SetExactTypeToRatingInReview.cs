#nullable disable

namespace StudentsHelper.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class SetExactTypeToRatingInReview : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Rating",
                table: "Reviews",
                type: "DECIMAL(2,1)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "DECIMAL(1,1)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Rating",
                table: "Reviews",
                type: "DECIMAL(1,1)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "DECIMAL(2,1)");
        }
    }
}
