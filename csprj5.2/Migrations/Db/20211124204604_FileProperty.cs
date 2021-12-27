using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace csprj5._2.Migrations.Db
{
    public partial class FileProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MonitoredFileProperties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MonitoredFileId = table.Column<int>(type: "INTEGER", nullable: true),
                    Property = table.Column<string>(type: "TEXT", nullable: true),
                    Hash = table.Column<string>(type: "TEXT", nullable: true),
                    HashDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonitoredFileProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MonitoredFileProperties_MonitoredFiles_MonitoredFileId",
                        column: x => x.MonitoredFileId,
                        principalTable: "MonitoredFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MonitoredFileProperties_MonitoredFileId",
                table: "MonitoredFileProperties",
                column: "MonitoredFileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MonitoredFileProperties");
        }
    }
}
