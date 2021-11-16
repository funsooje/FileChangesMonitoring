using Microsoft.EntityFrameworkCore.Migrations;

namespace csprj5._2.Migrations.Db
{
    public partial class MonitoredFileContentsUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Hash",
                table: "MonitoredFileContents",
                newName: "Content");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Content",
                table: "MonitoredFileContents",
                newName: "Hash");
        }
    }
}
