#nullable disable

namespace StudentsHelper.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddedChatEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consultation_Meeting_MeetingId",
                table: "Consultation");

            migrationBuilder.DropForeignKey(
                name: "FK_Consultation_Students_StudentId",
                table: "Consultation");

            migrationBuilder.DropForeignKey(
                name: "FK_Consultation_Subjects_SchoolSubjectId",
                table: "Consultation");

            migrationBuilder.DropForeignKey(
                name: "FK_Consultation_Teachers_TeacherId",
                table: "Consultation");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentsTransactions_Consultation_ConsultationId",
                table: "StudentsTransactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Meeting",
                table: "Meeting");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Consultation",
                table: "Consultation");

            migrationBuilder.RenameTable(
                name: "Meeting",
                newName: "Meetings");

            migrationBuilder.RenameTable(
                name: "Consultation",
                newName: "Consultations");

            migrationBuilder.RenameIndex(
                name: "IX_Consultation_TeacherId",
                table: "Consultations",
                newName: "IX_Consultations_TeacherId");

            migrationBuilder.RenameIndex(
                name: "IX_Consultation_StudentId",
                table: "Consultations",
                newName: "IX_Consultations_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_Consultation_SchoolSubjectId",
                table: "Consultations",
                newName: "IX_Consultations_SchoolSubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Consultation_MeetingId",
                table: "Consultations",
                newName: "IX_Consultations_MeetingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Meetings",
                table: "Meetings",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Consultations",
                table: "Consultations",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ChatGroups",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChatGroupUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ChatGroupId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatGroupUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatGroupUsers_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChatGroupUsers_ChatGroups_ChatGroupId",
                        column: x => x.ChatGroupId,
                        principalTable: "ChatGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    SenderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ChatGroupId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_AspNetUsers_SenderId",
                        column: x => x.SenderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Messages_ChatGroups_ChatGroupId",
                        column: x => x.ChatGroupId,
                        principalTable: "ChatGroups",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatGroups_IsDeleted",
                table: "ChatGroups",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ChatGroupUsers_ApplicationUserId",
                table: "ChatGroupUsers",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatGroupUsers_ChatGroupId",
                table: "ChatGroupUsers",
                column: "ChatGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ChatGroupId",
                table: "Messages",
                column: "ChatGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_IsDeleted",
                table: "Messages",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderId",
                table: "Messages",
                column: "SenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Consultations_Meetings_MeetingId",
                table: "Consultations",
                column: "MeetingId",
                principalTable: "Meetings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Consultations_Students_StudentId",
                table: "Consultations",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Consultations_Subjects_SchoolSubjectId",
                table: "Consultations",
                column: "SchoolSubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Consultations_Teachers_TeacherId",
                table: "Consultations",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentsTransactions_Consultations_ConsultationId",
                table: "StudentsTransactions",
                column: "ConsultationId",
                principalTable: "Consultations",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consultations_Meetings_MeetingId",
                table: "Consultations");

            migrationBuilder.DropForeignKey(
                name: "FK_Consultations_Students_StudentId",
                table: "Consultations");

            migrationBuilder.DropForeignKey(
                name: "FK_Consultations_Subjects_SchoolSubjectId",
                table: "Consultations");

            migrationBuilder.DropForeignKey(
                name: "FK_Consultations_Teachers_TeacherId",
                table: "Consultations");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentsTransactions_Consultations_ConsultationId",
                table: "StudentsTransactions");

            migrationBuilder.DropTable(
                name: "ChatGroupUsers");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "ChatGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Meetings",
                table: "Meetings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Consultations",
                table: "Consultations");

            migrationBuilder.RenameTable(
                name: "Meetings",
                newName: "Meeting");

            migrationBuilder.RenameTable(
                name: "Consultations",
                newName: "Consultation");

            migrationBuilder.RenameIndex(
                name: "IX_Consultations_TeacherId",
                table: "Consultation",
                newName: "IX_Consultation_TeacherId");

            migrationBuilder.RenameIndex(
                name: "IX_Consultations_StudentId",
                table: "Consultation",
                newName: "IX_Consultation_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_Consultations_SchoolSubjectId",
                table: "Consultation",
                newName: "IX_Consultation_SchoolSubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Consultations_MeetingId",
                table: "Consultation",
                newName: "IX_Consultation_MeetingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Meeting",
                table: "Meeting",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Consultation",
                table: "Consultation",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Consultation_Meeting_MeetingId",
                table: "Consultation",
                column: "MeetingId",
                principalTable: "Meeting",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Consultation_Students_StudentId",
                table: "Consultation",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Consultation_Subjects_SchoolSubjectId",
                table: "Consultation",
                column: "SchoolSubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Consultation_Teachers_TeacherId",
                table: "Consultation",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentsTransactions_Consultation_ConsultationId",
                table: "StudentsTransactions",
                column: "ConsultationId",
                principalTable: "Consultation",
                principalColumn: "Id");
        }
    }
}
