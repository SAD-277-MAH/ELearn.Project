using Microsoft.EntityFrameworkCore.Migrations;

namespace ELearn.Data.Migrations
{
    public partial class MigUpdateSettingSms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SmsApi",
                table: "Settings");

            migrationBuilder.AlterColumn<string>(
                name: "SmsSender",
                table: "Settings",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SmsAPIKey",
                table: "Settings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SmsSecurityKey",
                table: "Settings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SmsAPIKey",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "SmsSecurityKey",
                table: "Settings");

            migrationBuilder.AlterColumn<string>(
                name: "SmsSender",
                table: "Settings",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SmsApi",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
