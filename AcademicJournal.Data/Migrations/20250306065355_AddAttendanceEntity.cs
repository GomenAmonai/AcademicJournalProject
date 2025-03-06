using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcademicJournal.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAttendanceEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Lessons_LessonId1",
                table: "Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Subjects_SubjectId",
                table: "Attendances");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_SubjectId",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "Attendances");

            migrationBuilder.AlterColumn<int>(
                name: "LessonId1",
                table: "Attendances",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Lessons_LessonId1",
                table: "Attendances",
                column: "LessonId1",
                principalTable: "Lessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Lessons_LessonId1",
                table: "Attendances");

            migrationBuilder.AlterColumn<int>(
                name: "LessonId1",
                table: "Attendances",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "SubjectId",
                table: "Attendances",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_SubjectId",
                table: "Attendances",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Lessons_LessonId1",
                table: "Attendances",
                column: "LessonId1",
                principalTable: "Lessons",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Subjects_SubjectId",
                table: "Attendances",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
