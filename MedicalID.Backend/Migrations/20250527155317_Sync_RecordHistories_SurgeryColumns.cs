using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalID.Backend.Migrations
{
    /// <inheritdoc />
    public partial class Sync_RecordHistories_SurgeryColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Surgery",
                table: "RecordHistories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SurgeryNote",
                table: "RecordHistories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RecordHistoryFiles",
                columns: table => new
                {
                    FileID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecordHistoryID = table.Column<int>(type: "int", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecordHistoryFiles", x => x.FileID);
                    table.ForeignKey(
                        name: "FK_RecordHistoryFiles_RecordHistories_RecordHistoryID",
                        column: x => x.RecordHistoryID,
                        principalTable: "RecordHistories",
                        principalColumn: "RecordHistoryID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecordHistoryFiles_RecordHistoryID",
                table: "RecordHistoryFiles",
                column: "RecordHistoryID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecordHistoryFiles");

            migrationBuilder.DropColumn(
                name: "Surgery",
                table: "RecordHistories");

            migrationBuilder.DropColumn(
                name: "SurgeryNote",
                table: "RecordHistories");
        }
    }
}
