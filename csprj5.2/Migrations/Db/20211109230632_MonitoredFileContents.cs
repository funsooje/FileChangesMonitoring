using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace csprj5._2.Migrations.Db
{
    public partial class MonitoredFileContents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MonitoredFileContents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MonitoredFileId = table.Column<int>(type: "INTEGER", nullable: true),
                    Hash = table.Column<byte[]>(type: "BLOB", nullable: true),
                    ContentDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonitoredFileContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MonitoredFileContents_MonitoredFiles_MonitoredFileId",
                        column: x => x.MonitoredFileId,
                        principalTable: "MonitoredFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MonitoredFileContents_MonitoredFileId",
                table: "MonitoredFileContents",
                column: "MonitoredFileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MonitoredFileContents");
        }
    }
}
