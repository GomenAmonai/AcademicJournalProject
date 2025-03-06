using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcademicJournal.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddHomeworkEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileAttachmentPath",
                table: "Homeworks");

            migrationBuilder.RenameColumn(
                name: "DueDate",
                table: "Homeworks",
                newName: "Deadline");

            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "Homeworks",
                newName: "CreatedAt");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Homeworks",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "FileId",
                table: "Homeworks",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Homeworks_FileId",
                table: "Homeworks",
                column: "FileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Homeworks_Files_FileId",
                table: "Homeworks",
                column: "FileId",
                principalTable: "Files",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Homeworks_Files_FileId",
                table: "Homeworks");

            migrationBuilder.DropIndex(
                name: "IX_Homeworks_FileId",
                table: "Homeworks");

            migrationBuilder.DropColumn(
                name: "FileId",
                table: "Homeworks");

            migrationBuilder.RenameColumn(
                name: "Deadline",
                table: "Homeworks",
                newName: "DueDate");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Homeworks",
                newName: "CreationDate");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Homeworks",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "FileAttachmentPath",
                table: "Homeworks",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
