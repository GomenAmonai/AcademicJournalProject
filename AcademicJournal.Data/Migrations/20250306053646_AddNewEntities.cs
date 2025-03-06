using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AcademicJournal.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNewEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeacherSubjects_Groups_GroupId",
                table: "TeacherSubjects");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherSubjects_Subjects_SubjectId",
                table: "TeacherSubjects");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherSubjects_Teachers_TeacherId",
                table: "TeacherSubjects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeacherSubjects",
                table: "TeacherSubjects");

            migrationBuilder.RenameTable(
                name: "TeacherSubjects",
                newName: "TeacherSubject");

            migrationBuilder.RenameColumn(
                name: "FileSubmissionPath",
                table: "HomeworkSubmissions",
                newName: "Content");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Attendances",
                newName: "StudentId1");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Attendances",
                newName: "RecordedAt");

            migrationBuilder.RenameIndex(
                name: "IX_TeacherSubjects_TeacherId",
                table: "TeacherSubject",
                newName: "IX_TeacherSubject_TeacherId");

            migrationBuilder.RenameIndex(
                name: "IX_TeacherSubjects_SubjectId",
                table: "TeacherSubject",
                newName: "IX_TeacherSubject_SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_TeacherSubjects_GroupId",
                table: "TeacherSubject",
                newName: "IX_TeacherSubject_GroupId");

            migrationBuilder.AddColumn<int>(
                name: "UserId2",
                table: "Teachers",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GroupId1",
                table: "Students",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "Students",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Grade",
                table: "HomeworkSubmissions",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FileId",
                table: "HomeworkSubmissions",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FileId1",
                table: "HomeworkSubmissions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HomeworkId1",
                table: "HomeworkSubmissions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StudentId1",
                table: "HomeworkSubmissions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GroupId1",
                table: "Homeworks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SubjectId1",
                table: "Homeworks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StudentId1",
                table: "Grades",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SubjectId1",
                table: "Grades",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Attendances",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsPresent",
                table: "Attendances",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "LessonId",
                table: "Attendances",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LessonId1",
                table: "Attendances",
                type: "integer",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeacherSubject",
                table: "TeacherSubject",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FileName = table.Column<string>(type: "text", nullable: false),
                    OriginalFileName = table.Column<string>(type: "text", nullable: false),
                    ContentType = table.Column<string>(type: "text", nullable: false),
                    Data = table.Column<byte[]>(type: "bytea", nullable: false),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    UploadDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UploadedById = table.Column<int>(type: "integer", nullable: false),
                    UploadedById1 = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Files_Users_UploadedById",
                        column: x => x.UploadedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Files_Users_UploadedById1",
                        column: x => x.UploadedById1,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lessons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SubjectId = table.Column<int>(type: "integer", nullable: false),
                    TeacherId = table.Column<int>(type: "integer", nullable: false),
                    GroupId = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Topic = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    SubjectId1 = table.Column<int>(type: "integer", nullable: false),
                    TeacherId1 = table.Column<int>(type: "integer", nullable: false),
                    GroupId1 = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lessons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lessons_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lessons_Groups_GroupId1",
                        column: x => x.GroupId1,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lessons_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lessons_Subjects_SubjectId1",
                        column: x => x.SubjectId1,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lessons_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lessons_Teachers_TeacherId1",
                        column: x => x.TeacherId1,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsRead = table.Column<bool>(type: "boolean", nullable: false),
                    RelatedEntityType = table.Column<string>(type: "text", nullable: false),
                    RelatedEntityId = table.Column<int>(type: "integer", nullable: true),
                    UserId1 = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserId1",
                        column: x => x.UserId1,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_UserId2",
                table: "Teachers",
                column: "UserId2",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_GroupId1",
                table: "Students",
                column: "GroupId1");

            migrationBuilder.CreateIndex(
                name: "IX_Students_UserId1",
                table: "Students",
                column: "UserId1",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HomeworkSubmissions_FileId",
                table: "HomeworkSubmissions",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_HomeworkSubmissions_FileId1",
                table: "HomeworkSubmissions",
                column: "FileId1");

            migrationBuilder.CreateIndex(
                name: "IX_HomeworkSubmissions_HomeworkId1",
                table: "HomeworkSubmissions",
                column: "HomeworkId1");

            migrationBuilder.CreateIndex(
                name: "IX_HomeworkSubmissions_StudentId1",
                table: "HomeworkSubmissions",
                column: "StudentId1");

            migrationBuilder.CreateIndex(
                name: "IX_Homeworks_GroupId1",
                table: "Homeworks",
                column: "GroupId1");

            migrationBuilder.CreateIndex(
                name: "IX_Homeworks_SubjectId1",
                table: "Homeworks",
                column: "SubjectId1");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_StudentId1",
                table: "Grades",
                column: "StudentId1");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_SubjectId1",
                table: "Grades",
                column: "SubjectId1");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_LessonId",
                table: "Attendances",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_LessonId1",
                table: "Attendances",
                column: "LessonId1");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_StudentId1",
                table: "Attendances",
                column: "StudentId1");

            migrationBuilder.CreateIndex(
                name: "IX_Files_UploadedById",
                table: "Files",
                column: "UploadedById");

            migrationBuilder.CreateIndex(
                name: "IX_Files_UploadedById1",
                table: "Files",
                column: "UploadedById1");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_GroupId",
                table: "Lessons",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_GroupId1",
                table: "Lessons",
                column: "GroupId1");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_SubjectId",
                table: "Lessons",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_SubjectId1",
                table: "Lessons",
                column: "SubjectId1");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_TeacherId",
                table: "Lessons",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_TeacherId1",
                table: "Lessons",
                column: "TeacherId1");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId1",
                table: "Notifications",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Lessons_LessonId",
                table: "Attendances",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Lessons_LessonId1",
                table: "Attendances",
                column: "LessonId1",
                principalTable: "Lessons",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Students_StudentId1",
                table: "Attendances",
                column: "StudentId1",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_Students_StudentId1",
                table: "Grades",
                column: "StudentId1",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_Subjects_SubjectId1",
                table: "Grades",
                column: "SubjectId1",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Homeworks_Groups_GroupId1",
                table: "Homeworks",
                column: "GroupId1",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Homeworks_Subjects_SubjectId1",
                table: "Homeworks",
                column: "SubjectId1",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HomeworkSubmissions_Files_FileId",
                table: "HomeworkSubmissions",
                column: "FileId",
                principalTable: "Files",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HomeworkSubmissions_Files_FileId1",
                table: "HomeworkSubmissions",
                column: "FileId1",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HomeworkSubmissions_Homeworks_HomeworkId1",
                table: "HomeworkSubmissions",
                column: "HomeworkId1",
                principalTable: "Homeworks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HomeworkSubmissions_Students_StudentId1",
                table: "HomeworkSubmissions",
                column: "StudentId1",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Groups_GroupId1",
                table: "Students",
                column: "GroupId1",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Users_UserId1",
                table: "Students",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_Users_UserId2",
                table: "Teachers",
                column: "UserId2",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherSubject_Groups_GroupId",
                table: "TeacherSubject",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherSubject_Subjects_SubjectId",
                table: "TeacherSubject",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherSubject_Teachers_TeacherId",
                table: "TeacherSubject",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Lessons_LessonId",
                table: "Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Lessons_LessonId1",
                table: "Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Students_StudentId1",
                table: "Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_Grades_Students_StudentId1",
                table: "Grades");

            migrationBuilder.DropForeignKey(
                name: "FK_Grades_Subjects_SubjectId1",
                table: "Grades");

            migrationBuilder.DropForeignKey(
                name: "FK_Homeworks_Groups_GroupId1",
                table: "Homeworks");

            migrationBuilder.DropForeignKey(
                name: "FK_Homeworks_Subjects_SubjectId1",
                table: "Homeworks");

            migrationBuilder.DropForeignKey(
                name: "FK_HomeworkSubmissions_Files_FileId",
                table: "HomeworkSubmissions");

            migrationBuilder.DropForeignKey(
                name: "FK_HomeworkSubmissions_Files_FileId1",
                table: "HomeworkSubmissions");

            migrationBuilder.DropForeignKey(
                name: "FK_HomeworkSubmissions_Homeworks_HomeworkId1",
                table: "HomeworkSubmissions");

            migrationBuilder.DropForeignKey(
                name: "FK_HomeworkSubmissions_Students_StudentId1",
                table: "HomeworkSubmissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Groups_GroupId1",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Users_UserId1",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_Users_UserId2",
                table: "Teachers");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherSubject_Groups_GroupId",
                table: "TeacherSubject");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherSubject_Subjects_SubjectId",
                table: "TeacherSubject");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherSubject_Teachers_TeacherId",
                table: "TeacherSubject");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "Lessons");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Teachers_UserId2",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Students_GroupId1",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_UserId1",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_HomeworkSubmissions_FileId",
                table: "HomeworkSubmissions");

            migrationBuilder.DropIndex(
                name: "IX_HomeworkSubmissions_FileId1",
                table: "HomeworkSubmissions");

            migrationBuilder.DropIndex(
                name: "IX_HomeworkSubmissions_HomeworkId1",
                table: "HomeworkSubmissions");

            migrationBuilder.DropIndex(
                name: "IX_HomeworkSubmissions_StudentId1",
                table: "HomeworkSubmissions");

            migrationBuilder.DropIndex(
                name: "IX_Homeworks_GroupId1",
                table: "Homeworks");

            migrationBuilder.DropIndex(
                name: "IX_Homeworks_SubjectId1",
                table: "Homeworks");

            migrationBuilder.DropIndex(
                name: "IX_Grades_StudentId1",
                table: "Grades");

            migrationBuilder.DropIndex(
                name: "IX_Grades_SubjectId1",
                table: "Grades");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_LessonId",
                table: "Attendances");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_LessonId1",
                table: "Attendances");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_StudentId1",
                table: "Attendances");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeacherSubject",
                table: "TeacherSubject");

            migrationBuilder.DropColumn(
                name: "UserId2",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "GroupId1",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "FileId",
                table: "HomeworkSubmissions");

            migrationBuilder.DropColumn(
                name: "FileId1",
                table: "HomeworkSubmissions");

            migrationBuilder.DropColumn(
                name: "HomeworkId1",
                table: "HomeworkSubmissions");

            migrationBuilder.DropColumn(
                name: "StudentId1",
                table: "HomeworkSubmissions");

            migrationBuilder.DropColumn(
                name: "GroupId1",
                table: "Homeworks");

            migrationBuilder.DropColumn(
                name: "SubjectId1",
                table: "Homeworks");

            migrationBuilder.DropColumn(
                name: "StudentId1",
                table: "Grades");

            migrationBuilder.DropColumn(
                name: "SubjectId1",
                table: "Grades");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "IsPresent",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "LessonId",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "LessonId1",
                table: "Attendances");

            migrationBuilder.RenameTable(
                name: "TeacherSubject",
                newName: "TeacherSubjects");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "HomeworkSubmissions",
                newName: "FileSubmissionPath");

            migrationBuilder.RenameColumn(
                name: "StudentId1",
                table: "Attendances",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "RecordedAt",
                table: "Attendances",
                newName: "Date");

            migrationBuilder.RenameIndex(
                name: "IX_TeacherSubject_TeacherId",
                table: "TeacherSubjects",
                newName: "IX_TeacherSubjects_TeacherId");

            migrationBuilder.RenameIndex(
                name: "IX_TeacherSubject_SubjectId",
                table: "TeacherSubjects",
                newName: "IX_TeacherSubjects_SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_TeacherSubject_GroupId",
                table: "TeacherSubjects",
                newName: "IX_TeacherSubjects_GroupId");

            migrationBuilder.AlterColumn<int>(
                name: "Grade",
                table: "HomeworkSubmissions",
                type: "integer",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeacherSubjects",
                table: "TeacherSubjects",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherSubjects_Groups_GroupId",
                table: "TeacherSubjects",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherSubjects_Subjects_SubjectId",
                table: "TeacherSubjects",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherSubjects_Teachers_TeacherId",
                table: "TeacherSubjects",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
