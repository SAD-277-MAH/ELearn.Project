using Microsoft.EntityFrameworkCore.Migrations;

namespace ELearn.Data.Migrations
{
    public partial class MigExamPassingGrade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PassingGrade",
                table: "Exams",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PassingGrade",
                table: "Exams");
        }
    }
}
