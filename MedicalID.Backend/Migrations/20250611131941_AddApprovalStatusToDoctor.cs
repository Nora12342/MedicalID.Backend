using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalID.Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddApprovalStatusToDoctor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccessLogs_Patients_PatientID",
                table: "AccessLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientConditions_MedicalConditions_MedicalConditionConditionID",
                table: "PatientConditions");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientMedications_Medications_MedicationID",
                table: "PatientMedications");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientMedications_Patients_PatientID",
                table: "PatientMedications");

            migrationBuilder.DropForeignKey(
                name: "FK_RecordHistories_Patients_PatientID",
                table: "RecordHistories");

            migrationBuilder.DropIndex(
                name: "IX_RecordHistories_PatientID",
                table: "RecordHistories");

            migrationBuilder.DropIndex(
                name: "IX_PatientConditions_MedicalConditionConditionID",
                table: "PatientConditions");

            migrationBuilder.DropIndex(
                name: "IX_AccessLogs_PatientID",
                table: "AccessLogs");

            migrationBuilder.DropColumn(
                name: "PatientID",
                table: "RecordHistories");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "PatientMedications");

            migrationBuilder.DropColumn(
                name: "MedicalConditionConditionID",
                table: "PatientConditions");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "PatientConditions");

            migrationBuilder.DropColumn(
                name: "PatientID",
                table: "Medications");

            migrationBuilder.DropColumn(
                name: "PatientID",
                table: "AccessLogs");

            migrationBuilder.AddColumn<string>(
                name: "ApprovalStatus",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientMedications_Medications_MedicationID",
                table: "PatientMedications",
                column: "MedicationID",
                principalTable: "Medications",
                principalColumn: "MedicationID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientMedications_Patients_PatientID",
                table: "PatientMedications",
                column: "PatientID",
                principalTable: "Patients",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientMedications_Medications_MedicationID",
                table: "PatientMedications");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientMedications_Patients_PatientID",
                table: "PatientMedications");

            migrationBuilder.DropColumn(
                name: "ApprovalStatus",
                table: "Doctors");

            migrationBuilder.AddColumn<int>(
                name: "PatientID",
                table: "RecordHistories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "PatientMedications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "MedicalConditionConditionID",
                table: "PatientConditions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "PatientConditions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PatientID",
                table: "Medications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PatientID",
                table: "AccessLogs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecordHistories_PatientID",
                table: "RecordHistories",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientConditions_MedicalConditionConditionID",
                table: "PatientConditions",
                column: "MedicalConditionConditionID");

            migrationBuilder.CreateIndex(
                name: "IX_AccessLogs_PatientID",
                table: "AccessLogs",
                column: "PatientID");

            migrationBuilder.AddForeignKey(
                name: "FK_AccessLogs_Patients_PatientID",
                table: "AccessLogs",
                column: "PatientID",
                principalTable: "Patients",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientConditions_MedicalConditions_MedicalConditionConditionID",
                table: "PatientConditions",
                column: "MedicalConditionConditionID",
                principalTable: "MedicalConditions",
                principalColumn: "ConditionID");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientMedications_Medications_MedicationID",
                table: "PatientMedications",
                column: "MedicationID",
                principalTable: "Medications",
                principalColumn: "MedicationID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientMedications_Patients_PatientID",
                table: "PatientMedications",
                column: "PatientID",
                principalTable: "Patients",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecordHistories_Patients_PatientID",
                table: "RecordHistories",
                column: "PatientID",
                principalTable: "Patients",
                principalColumn: "ID");
        }
    }
}
