using Microsoft.EntityFrameworkCore.Migrations;

namespace csprj5._2.Migrations.Db
{
    public partial class MonitoredFileProps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "MonitorJustHash",
                table: "MonitoredFiles",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "MonitorProperties",
                table: "MonitoredFiles",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MonitorJustHash",
                table: "MonitoredFiles");

            migrationBuilder.DropColumn(
                name: "MonitorProperties",
                table: "MonitoredFiles");
        }
    }
}
