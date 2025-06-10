using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalID.Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddPatientIdToMedicationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccessLogs_Patients_PatientID",
                table: "AccessLogs");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "PatientAllergies");

            migrationBuilder.AlterColumn<int>(
                name: "PatientID",
                table: "AccessLogs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_AccessLogs_MedicalID",
                table: "AccessLogs",
                column: "MedicalID");

            migrationBuilder.AddForeignKey(
                name: "FK_AccessLogs_Patients_MedicalID",
                table: "AccessLogs",
                column: "MedicalID",
                principalTable: "Patients",
                principalColumn: "MedicalID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AccessLogs_Patients_PatientID",
                table: "AccessLogs",
                column: "PatientID",
                principalTable: "Patients",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccessLogs_Patients_MedicalID",
                table: "AccessLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_AccessLogs_Patients_PatientID",
                table: "AccessLogs");

            migrationBuilder.DropIndex(
                name: "IX_AccessLogs_MedicalID",
                table: "AccessLogs");

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "PatientAllergies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "PatientID",
                table: "AccessLogs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AccessLogs_Patients_PatientID",
                table: "AccessLogs",
                column: "PatientID",
                principalTable: "Patients",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
