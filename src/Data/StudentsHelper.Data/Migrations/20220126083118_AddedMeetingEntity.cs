using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentsHelper.Data.Migrations
{
    public partial class AddedMeetingEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MeetingId",
                table: "Consultation",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Meeting",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meeting", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Consultation_MeetingId",
                table: "Consultation",
                column: "MeetingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Consultation_Meeting_MeetingId",
                table: "Consultation",
                column: "MeetingId",
                principalTable: "Meeting",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consultation_Meeting_MeetingId",
                table: "Consultation");

            migrationBuilder.DropTable(
                name: "Meeting");

            migrationBuilder.DropIndex(
                name: "IX_Consultation_MeetingId",
                table: "Consultation");

            migrationBuilder.DropColumn(
                name: "MeetingId",
                table: "Consultation");
        }
    }
}
