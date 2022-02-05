using System;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentsHelper.Data.Migrations
{
    public partial class ChangedConsultationTypeToBaseModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Consultation_IsDeleted",
                table: "Consultation");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Consultation");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Consultation");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Consultation",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Consultation",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Consultation_IsDeleted",
                table: "Consultation",
                column: "IsDeleted");
        }
    }
}
