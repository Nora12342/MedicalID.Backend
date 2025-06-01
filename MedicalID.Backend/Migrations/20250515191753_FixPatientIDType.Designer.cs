using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalID.Backend.Migrations
{
    public partial class FixPatientIDType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop foreign keys referencing Patients.PatientID
            migrationBuilder.DropForeignKey(
                name: "FK_RecordHistory_Patients_PatientID",
                table: "RecordHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_Medications_Patients_PatientID",
                table: "Medications");

            migrationBuilder.DropForeignKey(
                name: "FK_AccessLogs_Patients_PatientID",
                table: "AccessLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Allergies_Patients_PatientID",
                table: "Allergies");

            // Drop primary key on Patients
            migrationBuilder.DropPrimaryKey(
                name: "PK_Patients",
                table: "Patients");

            // Alter column PatientID in Patients to nvarchar(450)
            migrationBuilder.AlterColumn<string>(
                name: "PatientID",
                table: "Patients",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            // Recreate primary key on Patients.PatientID
            migrationBuilder.AddPrimaryKey(
                name: "PK_Patients",
                table: "Patients",
                column: "PatientID");

            // Alter PatientID columns in related tables to nvarchar(450)

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

            // Recreate foreign keys
            migrationBuilder.AddForeignKey(
                name: "FK_RecordHistory_Patients_PatientID",
                table: "RecordHistory",
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop foreign keys referencing Patients.PatientID
            migrationBuilder.DropForeignKey(
                name: "FK_RecordHistory_Patients_PatientID",
                table: "RecordHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_Medications_Patients_PatientID",
                table: "Medications");

            migrationBuilder.DropForeignKey(
                name: "FK_AccessLogs_Patients_PatientID",
                table: "AccessLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Allergies_Patients_PatientID",
                table: "Allergies");

            // Drop primary key on Patients
            migrationBuilder.DropPrimaryKey(
                name: "PK_Patients",
                table: "Patients");

            // Alter column PatientID in Patients back to int
            migrationBuilder.AlterColumn<int>(
                name: "PatientID",
                table: "Patients",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            // Recreate primary key on Patients.PatientID
            migrationBuilder.AddPrimaryKey(
                name: "PK_Patients",
                table: "Patients",
                column: "PatientID");

            // Alter PatientID columns in related tables back to int

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

            // Recreate foreign keys
            migrationBuilder.AddForeignKey(
                name: "FK_RecordHistory_Patients_PatientID",
                table: "RecordHistory",
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
        }
    }
}

