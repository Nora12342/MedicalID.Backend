using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalID.Backend.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    CityID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.CityID);
                });

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    RegionID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CityID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.RegionID);
                    table.ForeignKey(
                        name: "FK_Regions_Cities_CityID",
                        column: x => x.CityID,
                        principalTable: "Cities",
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
                        name: "FK_Doctors_Regions_RegionID",
                        column: x => x.RegionID,
                        principalTable: "Regions",
                        principalColumn: "RegionID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Hospitals",
                columns: table => new
                {
                    HospitalID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegionID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hospitals", x => x.HospitalID);
                    table.ForeignKey(
                        name: "FK_Hospitals_Regions_RegionID",
                        column: x => x.RegionID,
                        principalTable: "Regions",
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
                        name: "FK_Patients_Regions_RegionID",
                        column: x => x.RegionID,
                        principalTable: "Regions",
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
                name: "AskDoctors",
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
                    table.PrimaryKey("PK_AskDoctors", x => new { x.PatientID, x.MessageID });
                    table.ForeignKey(
                        name: "FK_AskDoctors_Doctors_DoctorID",
                        column: x => x.DoctorID,
                        principalTable: "Doctors",
                        principalColumn: "DoctorID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AskDoctors_Patients_PatientID",
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
                name: "RecordHistories",
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
                    table.PrimaryKey("PK_RecordHistories", x => new { x.PatientID, x.RecordID });
                    table.ForeignKey(
                        name: "FK_RecordHistories_AccessLogs_AccessLogID",
                        column: x => x.AccessLogID,
                        principalTable: "AccessLogs",
                        principalColumn: "LogID");
                    table.ForeignKey(
                        name: "FK_RecordHistories_Doctors_DoctorID",
                        column: x => x.DoctorID,
                        principalTable: "Doctors",
                        principalColumn: "DoctorID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecordHistories_Patients_PatientID",
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
                name: "IX_AskDoctors_DoctorID",
                table: "AskDoctors",
                column: "DoctorID");

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_RegionID",
                table: "Doctors",
                column: "RegionID");

            migrationBuilder.CreateIndex(
                name: "IX_Hospitals_RegionID",
                table: "Hospitals",
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
                name: "IX_RecordHistories_AccessLogID",
                table: "RecordHistories",
                column: "AccessLogID");

            migrationBuilder.CreateIndex(
                name: "IX_RecordHistories_DoctorID",
                table: "RecordHistories",
                column: "DoctorID");

            migrationBuilder.CreateIndex(
                name: "IX_Regions_CityID",
                table: "Regions",
                column: "CityID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Allergies");

            migrationBuilder.DropTable(
                name: "AskDoctors");

            migrationBuilder.DropTable(
                name: "Hospitals");

            migrationBuilder.DropTable(
                name: "MedicalConditions");

            migrationBuilder.DropTable(
                name: "Medications");

            migrationBuilder.DropTable(
                name: "RecordHistories");

            migrationBuilder.DropTable(
                name: "AccessLogs");

            migrationBuilder.DropTable(
                name: "Doctors");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropTable(
                name: "Cities");
        }
    }
}
