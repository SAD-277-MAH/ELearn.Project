using Microsoft.EntityFrameworkCore.Migrations;

namespace ELearn.Data.Migrations
{
    public partial class MigFixCourse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Courses_PrerequisitesId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_PrerequisitesId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "PrerequisitesId",
                table: "Courses");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Teachers",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Specialty",
                table: "Teachers",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Specialty",
                table: "Teachers");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Teachers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrerequisitesId",
                table: "Courses",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Courses_PrerequisitesId",
                table: "Courses",
                column: "PrerequisitesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Courses_PrerequisitesId",
                table: "Courses",
                column: "PrerequisitesId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
