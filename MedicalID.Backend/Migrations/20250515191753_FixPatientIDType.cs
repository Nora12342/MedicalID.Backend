using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

public partial class FixPatientIDType : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // === DROP FOREIGN KEYS ===
        migrationBuilder.DropForeignKey("FK_AccessLogs_Patients_PatientID", "AccessLogs");
        migrationBuilder.DropForeignKey("FK_Allergies_Patients_PatientID", "Allergies");
        migrationBuilder.DropForeignKey("FK_MedicalConditions_Patients_PatientID", "MedicalConditions");
        migrationBuilder.DropForeignKey("FK_Medications_Patients_PatientID", "Medications");
        migrationBuilder.DropForeignKey("FK_RecordHistory_Patients_PatientID", "RecordHistory");

        // === DROP PRIMARY KEYS if they involve PatientID ===
        migrationBuilder.DropPrimaryKey("PK_Allergies", "Allergies");

        // === ALTER PatientID columns ===
        migrationBuilder.AlterColumn<string>(
            name: "PatientID",
            table: "Patients",
            type: "nvarchar(450)",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "int");

        migrationBuilder.AlterColumn<string>(
            name: "PatientID",
            table: "AccessLogs",
            type: "nvarchar(450)",
            nullable: true,
            oldClrType: typeof(int),
            oldType: "int",
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "PatientID",
            table: "Allergies",
            type: "nvarchar(450)",
            nullable: true,
            oldClrType: typeof(int),
            oldType: "int",
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "PatientID",
            table: "MedicalConditions",
            type: "nvarchar(450)",
            nullable: true,
            oldClrType: typeof(int),
            oldType: "int",
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "PatientID",
            table: "Medications",
            type: "nvarchar(450)",
            nullable: true,
            oldClrType: typeof(int),
            oldType: "int",
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "PatientID",
            table: "RecordHistory",
            type: "nvarchar(450)",
            nullable: true,
            oldClrType: typeof(int),
            oldType: "int",
            oldNullable: true);

        // === RE-ADD PRIMARY KEYS ===
        migrationBuilder.AddPrimaryKey(
            name: "PK_Allergies",
            table: "Allergies",
            column: "AllergyID");

        // === RE-ADD FOREIGN KEYS ===
        migrationBuilder.AddForeignKey(
            name: "FK_AccessLogs_Patients_PatientID",
            table: "AccessLogs",
            column: "PatientID",
            principalTable: "Patients",
            principalColumn: "PatientID");

        migrationBuilder.AddForeignKey(
            name: "FK_Allergies_Patients_PatientID",
            table: "Allergies",
            column: "PatientID",
            principalTable: "Patients",
            principalColumn: "PatientID");

        migrationBuilder.AddForeignKey(
            name: "FK_MedicalConditions_Patients_PatientID",
            table: "MedicalConditions",
            column: "PatientID",
            principalTable: "Patients",
            principalColumn: "PatientID");

        migrationBuilder.AddForeignKey(
            name: "FK_Medications_Patients_PatientID",
            table: "Medications",
            column: "PatientID",
            principalTable: "Patients",
            principalColumn: "PatientID");

        migrationBuilder.AddForeignKey(
            name: "FK_RecordHistory_Patients_PatientID",
            table: "RecordHistory",
            column: "PatientID",
            principalTable: "Patients",
            principalColumn: "PatientID");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // === Drop FK for rollback ===
        migrationBuilder.DropForeignKey("FK_AccessLogs_Patients_PatientID", "AccessLogs");
        migrationBuilder.DropForeignKey("FK_Allergies_Patients_PatientID", "Allergies");
        migrationBuilder.DropForeignKey("FK_MedicalConditions_Patients_PatientID", "MedicalConditions");
        migrationBuilder.DropForeignKey("FK_Medications_Patients_PatientID", "Medications");
        migrationBuilder.DropForeignKey("FK_RecordHistory_Patients_PatientID", "RecordHistory");

        migrationBuilder.DropPrimaryKey("PK_Allergies", "Allergies");

        // === Revert column types ===
        migrationBuilder.AlterColumn<int>(
            name: "PatientID",
            table: "Patients",
            type: "int",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(450)");

        migrationBuilder.AlterColumn<int>(
            name: "PatientID",
            table: "AccessLogs",
            type: "int",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(450)",
            oldNullable: true);

        migrationBuilder.AlterColumn<int>(
            name: "PatientID",
            table: "Allergies",
            type: "int",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(450)",
            oldNullable: true);

        migrationBuilder.AlterColumn<int>(
            name: "PatientID",
            table: "MedicalConditions",
            type: "int",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(450)",
            oldNullable: true);

        migrationBuilder.AlterColumn<int>(
            name: "PatientID",
            table: "Medications",
            type: "int",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(450)",
            oldNullable: true);

        migrationBuilder.AlterColumn<int>(
            name: "PatientID",
            table: "RecordHistory",
            type: "int",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(450)",
            oldNullable: true);

        // Re-add PK
        migrationBuilder.AddPrimaryKey(
            name: "PK_Allergies",
            table: "Allergies",
            column: "AllergyID");

        // Re-add FKs
        migrationBuilder.AddForeignKey(
            name: "FK_AccessLogs_Patients_PatientID",
            table: "AccessLogs",
            column: "PatientID",
            principalTable: "Patients",
            principalColumn: "PatientID");

        migrationBuilder.AddForeignKey(
            name: "FK_Allergies_Patients_PatientID",
            table: "Allergies",
            column: "PatientID",
            principalTable: "Patients",
            principalColumn: "PatientID");

        migrationBuilder.AddForeignKey(
            name: "FK_MedicalConditions_Patients_PatientID",
            table: "MedicalConditions",
            column: "PatientID",
            principalTable: "Patients",
            principalColumn: "PatientID");

        migrationBuilder.AddForeignKey(
            name: "FK_Medications_Patients_PatientID",
            table: "Medications",
            column: "PatientID",
            principalTable: "Patients",
            principalColumn: "PatientID");

        migrationBuilder.AddForeignKey(
            name: "FK_RecordHistory_Patients_PatientID",
            table: "RecordHistory",
            column: "PatientID",
            principalTable: "Patients",
            principalColumn: "PatientID");
    }
}



