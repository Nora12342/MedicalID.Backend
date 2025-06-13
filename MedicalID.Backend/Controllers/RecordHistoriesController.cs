using MedicalID.Backend.Data;
using MedicalID.Backend.Dtos.RecordHistory;
using MedicalID.Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MedicalID.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecordHistoriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RecordHistoriesController(AppDbContext context)
        {
            _context = context;
        }

        // Patients can view their own records via MedicalID
        [Authorize(Roles = "Patient")]
        [HttpGet("my")]
        public async Task<IActionResult> GetMyRecordHistory()
        {
            var patientIdFromTokenStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(patientIdFromTokenStr))
                return Unauthorized("Authentication failed: Patient ID missing in token.");

            if (!int.TryParse(patientIdFromTokenStr, out int patientIntId))
            {
                return Unauthorized("Authentication failed: Invalid patient ID format in token.");
            }

            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.ID == patientIntId);
            if (patient == null)
                return Unauthorized("Patient not found in the database.");

            var patientMedicalId = patient.MedicalID;
            if (string.IsNullOrWhiteSpace(patientMedicalId))
                return BadRequest("Patient's MedicalID is not set. Cannot retrieve record history.");

            var records = await _context.RecordHistories
                .Where(r => r.MedicalID == patientMedicalId)
                .Include(r => r.Doctor) // Include related Doctor data
                .Include(r => r.AccessLog) // Include related AccessLog data
                                           // .Include(r => r.Files) // Uncomment if you need to include files and have a DTO for them
                .Select(r => new RecordHistoryDTO // Correctly map to the updated DTO
                {
                    RecordHistoryID = r.RecordHistoryID,
                    MedicalID = r.MedicalID,
                    DoctorID = r.DoctorID,
                    LogID = r.LogID,
                    DiagnosisNotes = r.DiagnosisNotes,
                    TreatmentPlan = r.TreatmentPlan,
                    CreateTime = r.CreateTime,
                    UpdateTime = r.UpdateTime,
                    Surgery = r.Surgery,
                    SurgeryNote = r.SurgeryNote,
                    DoctorName = r.Doctor != null ? r.Doctor.FName : "N/A", // Map Doctor's full name
                    // Files = r.Files != null ? r.Files.Select(f => new RecordHistoryFileDto { /* map properties */ }).ToList() : null // Example for mapping files
                })
                .ToListAsync();

            if (!records.Any())
            {
                return NotFound("No record history found for this patient with the specified MedicalID.");
            }

            return Ok(records);
        }

        // Doctors can create record history (MedicalID-based)
        [Authorize(Roles = "Doctor")]
        [HttpPost]
        public async Task<IActionResult> AddRecordHistory([FromBody] RecordHistoryPostDto dto)
        {
            var doctorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (doctorId != dto.DoctorID)
                return Forbid("You cannot use another doctor's ID.");

            var accessGranted = await _context.AccessLogs
                .AnyAsync(a => a.DoctorID == dto.DoctorID &&
                               a.MedicalID == dto.MedicalID &&
                               a.AccessGranted == true);

            if (!accessGranted)
                return StatusCode(403, "Access not granted by the patient.");

            var newRecord = new RecordHistory
            {
                MedicalID = dto.MedicalID,
                DoctorID = dto.DoctorID,
                LogID = dto.LogID,
                DiagnosisNotes = dto.DiagnosisNotes,
                TreatmentPlan = dto.TreatmentPlan,
                Surgery = dto.Surgery,
                SurgeryNote = dto.SurgeryNote,
                CreateTime = DateTime.UtcNow
            };

            _context.RecordHistories.Add(newRecord);
            await _context.SaveChangesAsync();
            return Ok("Record added successfully.");
        }

        // Doctors can update record history
        // Note: The input DTO for PUT/Update should match the fields you allow to be updated.
        // Your current RecordHistoryDTO is missing Surgery and SurgeryNote, so be careful here.
        // I will assume you want to allow updating Surgery and SurgeryNote, so I'll add them to the DTO for PUT requests.
        // If not, you might need a separate UpdateRecordHistoryDto or adjust this DTO.
        [Authorize(Roles = "Doctor")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRecordHistory(int id, [FromBody] RecordHistoryPostDto dto) // Using RecordHistoryPostDto for consistency for update fields
        {
            var doctorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var record = await _context.RecordHistories.FindAsync(id);
            if (record == null) return NotFound("Record not found.");

            if (record.DoctorID != doctorId)
                return Forbid("You cannot update another doctor’s record.");

            var accessGranted = await _context.AccessLogs
                .AnyAsync(a => a.DoctorID == doctorId &&
                               a.MedicalID == record.MedicalID &&
                               a.AccessGranted == true);

            if (!accessGranted)
                return StatusCode(403, "Access not granted by the patient.");

            // Update allowed fields based on the DTO
            record.DiagnosisNotes = dto.DiagnosisNotes;
            record.TreatmentPlan = dto.TreatmentPlan;
            record.Surgery = dto.Surgery; // Now included in the PostDto
            record.SurgeryNote = dto.SurgeryNote; // Now included in the PostDto
            record.UpdateTime = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Ok("Record updated successfully.");
        }

        // Doctors can view records by MedicalID
        [Authorize(Roles = "Doctor")]
        [HttpGet("patient/{medicalId}")]
        public async Task<IActionResult> GetPatientRecords(string medicalId)
        {
            var doctorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var accessGranted = await _context.AccessLogs
                .AnyAsync(a => a.DoctorID == doctorId &&
                               a.MedicalID == medicalId &&
                               a.AccessGranted == true);

            if (!accessGranted)
                return StatusCode(403, "Access not granted by the patient.");

            var records = await _context.RecordHistories
                .Where(r => r.MedicalID == medicalId)
                .Include(r => r.AccessLog)
                .Include(r => r.Doctor)
                // .Include(r => r.Files) // Uncomment if you need to include files
                .Select(r => new RecordHistoryDTO // Correctly map to the updated DTO
                {
                    RecordHistoryID = r.RecordHistoryID,
                    MedicalID = r.MedicalID,
                    DoctorID = r.DoctorID,
                    LogID = r.LogID,
                    DiagnosisNotes = r.DiagnosisNotes,
                    TreatmentPlan = r.TreatmentPlan,
                    CreateTime = r.CreateTime,
                    UpdateTime = r.UpdateTime,
                    Surgery = r.Surgery,
                    SurgeryNote = r.SurgeryNote,
                    DoctorName = r.Doctor != null ? r.Doctor.FName : "N/A",
                    // Files = r.Files != null ? r.Files.Select(f => new RecordHistoryFileDto { /* map properties */ }).ToList() : null // Example for mapping files
                })
                .ToListAsync();

            if (!records.Any())
            {
                return NotFound("No record history found for this patient.");
            }

            return Ok(records);
        }

        [HttpPost("upload-file/{recordId}")]
        public async Task<IActionResult> UploadFile(int recordId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var record = await _context.RecordHistories.FindAsync(recordId);
            if (record == null)
                return NotFound("Record not found.");

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            Directory.CreateDirectory(uploadsFolder);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var newFile = new RecordHistoryFile
            {
                RecordHistoryID = recordId,
                FilePath = "/uploads/" + fileName,
                UploadedAt = DateTime.UtcNow
            };

            _context.RecordHistoryFiles.Add(newFile);
            await _context.SaveChangesAsync();

            return Ok(new { message = "File uploaded", filePath = newFile.FilePath });
        }
        [Authorize(Roles = "Patient")]
        [HttpGet("labtests")]
        public async Task<IActionResult> GetMyLabTests()
        {
            var patientIdFromTokenStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(patientIdFromTokenStr))
                return Unauthorized("Authentication failed: Patient ID missing in token.");

            if (!int.TryParse(patientIdFromTokenStr, out int patientIntId))
                return Unauthorized("Invalid patient ID format in token.");

            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.ID == patientIntId);
            if (patient == null)
                return Unauthorized("Patient not found.");

            var medicalId = patient.MedicalID;

            var files = await _context.RecordHistories
                .Where(r => r.MedicalID == medicalId)
                .Include(r => r.RecordHistoryFiles)
                .SelectMany(r => r.RecordHistoryFiles.Select(f => new {
                    f.FileID,
                    f.FilePath,
                    f.UploadedAt
                }))
                .ToListAsync();

            if (files.Count == 0)
                return Ok(new { message = "No lab test files attached.", labTestFiles = new List<object>() });

            return Ok(new { labTestFiles = files });
        }

        [Authorize(Roles = "Patient")]
        [HttpDelete("labtests/{fileId}")]
        public async Task<IActionResult> DeleteLabTestFile(int fileId)
        {
            var file = await _context.RecordHistoryFiles.FindAsync(fileId);
            if (file == null)
                return NotFound("File not found.");

            // احذف من السيرفر كمان لو موجود
            var physicalPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", file.FilePath.TrimStart('/'));
            if (System.IO.File.Exists(physicalPath))
            {
                System.IO.File.Delete(physicalPath);
            }

            _context.RecordHistoryFiles.Remove(file);
            await _context.SaveChangesAsync();

            return Ok("File deleted successfully.");
        }

        [Authorize(Roles = "Doctor")]
        [HttpGet("labtests/{medicalId}")]
        public async Task<IActionResult> GetPatientLabTests(string medicalId)
        {
            var doctorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var accessGranted = await _context.AccessLogs.AnyAsync(a =>
                a.DoctorID == doctorId && a.MedicalID == medicalId && a.AccessGranted == true);

            if (!accessGranted)
                return Forbid("Access not granted by patient.");

            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.MedicalID == medicalId);
            if (patient == null)
                return NotFound("Patient not found.");

            var files = await _context.RecordHistories
                .Where(r => r.MedicalID == medicalId)
                .Include(r => r.RecordHistoryFiles)
                .SelectMany(r => r.RecordHistoryFiles.Select(f => new {
                    f.FileID,
                    f.FilePath,
                    f.UploadedAt
                }))
                .ToListAsync();

            return Ok(new { labTestFiles = files });
        }



        // Doctors can delete a record
        [Authorize(Roles = "Doctor")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecordHistory(int id)
        {
            var doctorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var record = await _context.RecordHistories.FindAsync(id);
            if (record == null) return NotFound("Record not found.");

            var accessGranted = await _context.AccessLogs
                .AnyAsync(a => a.DoctorID == doctorId &&
                               a.MedicalID == record.MedicalID &&
                               a.AccessGranted == true);

            if (!accessGranted)
                return StatusCode(403, "Access not granted by the patient.");

            _context.RecordHistories.Remove(record);
            await _context.SaveChangesAsync();
            return Ok("Record deleted successfully.");
        }
    }
}

