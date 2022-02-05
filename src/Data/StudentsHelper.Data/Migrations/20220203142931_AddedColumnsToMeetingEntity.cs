#nullable disable

namespace StudentsHelper.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddedColumnsToMeetingEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DurationInMinutes",
                table: "Meeting",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "StudentLastActivity",
                table: "Meeting",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TeacherLastActivity",
                table: "Meeting",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DurationInMinutes",
                table: "Meeting");

            migrationBuilder.DropColumn(
                name: "StudentLastActivity",
                table: "Meeting");

            migrationBuilder.DropColumn(
                name: "TeacherLastActivity",
                table: "Meeting");
        }
    }
}
