using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalID.Backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    CityID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.CityID);
                });

            migrationBuilder.CreateTable(
                name: "Region",
                columns: table => new
                {
                    RegionID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CityID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Region", x => x.RegionID);
                    table.ForeignKey(
                        name: "FK_Region_City_CityID",
                        column: x => x.CityID,
                        principalTable: "City",
                        principalColumn: "CityID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Doctors",
                columns: table => new
                {
                    DoctorID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Specialization = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegionID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctors", x => x.DoctorID);
                    table.ForeignKey(
                        name: "FK_Doctors_Region_RegionID",
                        column: x => x.RegionID,
                        principalTable: "Region",
                        principalColumn: "RegionID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Hospital",
                columns: table => new
                {
                    HospitalID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegionID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hospital", x => x.HospitalID);
                    table.ForeignKey(
                        name: "FK_Hospital_Region_RegionID",
                        column: x => x.RegionID,
                        principalTable: "Region",
                        principalColumn: "RegionID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    PatientID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MedicalID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BloodType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmergencyContact = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrganDonorStatus = table.Column<bool>(type: "bit", nullable: false),
                    RegionID = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.PatientID);
                    table.ForeignKey(
                        name: "FK_Patients_Region_RegionID",
                        column: x => x.RegionID,
                        principalTable: "Region",
                        principalColumn: "RegionID");
                });

            migrationBuilder.CreateTable(
                name: "AccessLogs",
                columns: table => new
                {
                    LogID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoctorID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PatientID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AccessTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Purpose = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessLogs", x => x.LogID);
                    table.ForeignKey(
                        name: "FK_AccessLogs_Doctors_DoctorID",
                        column: x => x.DoctorID,
                        principalTable: "Doctors",
                        principalColumn: "DoctorID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccessLogs_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Allergies",
                columns: table => new
                {
                    PatientID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Allergen = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Reaction = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Severity = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Allergies", x => new { x.PatientID, x.Allergen });
                    table.ForeignKey(
                        name: "FK_Allergies_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AskDoctor",
                columns: table => new
                {
                    MessageID = table.Column<int>(type: "int", nullable: false),
                    PatientID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DoctorID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Response = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RepliedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AskDoctor", x => new { x.PatientID, x.MessageID });
                    table.ForeignKey(
                        name: "FK_AskDoctor_Doctors_DoctorID",
                        column: x => x.DoctorID,
                        principalTable: "Doctors",
                        principalColumn: "DoctorID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AskDoctor_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicalConditions",
                columns: table => new
                {
                    MedConditionID = table.Column<int>(type: "int", nullable: false),
                    PatientID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ConditionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiagnosedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalConditions", x => new { x.PatientID, x.MedConditionID });
                    table.ForeignKey(
                        name: "FK_MedicalConditions_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Medications",
                columns: table => new
                {
                    MedicationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MedicationName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dosage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Frequency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrescribedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medications", x => x.MedicationID);
                    table.ForeignKey(
                        name: "FK_Medications_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecordHistory",
                columns: table => new
                {
                    RecordID = table.Column<int>(type: "int", nullable: false),
                    PatientID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DoctorID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AccessLogID = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecordHistory", x => new { x.PatientID, x.RecordID });
                    table.ForeignKey(
                        name: "FK_RecordHistory_AccessLogs_AccessLogID",
                        column: x => x.AccessLogID,
                        principalTable: "AccessLogs",
                        principalColumn: "LogID");
                    table.ForeignKey(
                        name: "FK_RecordHistory_Doctors_DoctorID",
                        column: x => x.DoctorID,
                        principalTable: "Doctors",
                        principalColumn: "DoctorID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecordHistory_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccessLogs_DoctorID",
                table: "AccessLogs",
                column: "DoctorID");

            migrationBuilder.CreateIndex(
                name: "IX_AccessLogs_PatientID",
                table: "AccessLogs",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_AskDoctor_DoctorID",
                table: "AskDoctor",
                column: "DoctorID");

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_RegionID",
                table: "Doctors",
                column: "RegionID");

            migrationBuilder.CreateIndex(
                name: "IX_Hospital_RegionID",
                table: "Hospital",
                column: "RegionID");

            migrationBuilder.CreateIndex(
                name: "IX_Medications_PatientID",
                table: "Medications",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_RegionID",
                table: "Patients",
                column: "RegionID");

            migrationBuilder.CreateIndex(
                name: "IX_RecordHistory_AccessLogID",
                table: "RecordHistory",
                column: "AccessLogID");

            migrationBuilder.CreateIndex(
                name: "IX_RecordHistory_DoctorID",
                table: "RecordHistory",
                column: "DoctorID");

            migrationBuilder.CreateIndex(
                name: "IX_Region_CityID",
                table: "Region",
                column: "CityID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Allergies");

            migrationBuilder.DropTable(
                name: "AskDoctor");

            migrationBuilder.DropTable(
                name: "Hospital");

            migrationBuilder.DropTable(
                name: "MedicalConditions");

            migrationBuilder.DropTable(
                name: "Medications");

            migrationBuilder.DropTable(
                name: "RecordHistory");

            migrationBuilder.DropTable(
                name: "AccessLogs");

            migrationBuilder.DropTable(
                name: "Doctors");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "Region");

            migrationBuilder.DropTable(
                name: "City");
        }
    }
}
