using Microsoft.EntityFrameworkCore.Migrations;

namespace csprj5._2.Migrations.Db
{
    public partial class MonitorFileEnable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Enabled",
                table: "MonitoredFiles",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Enabled",
                table: "MonitoredFiles");
        }
    }
}
