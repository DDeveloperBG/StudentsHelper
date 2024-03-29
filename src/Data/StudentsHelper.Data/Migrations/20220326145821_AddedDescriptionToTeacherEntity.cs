﻿#nullable disable

namespace StudentsHelper.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddedDescriptionToTeacherEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Teachers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Teachers");
        }
    }
}
