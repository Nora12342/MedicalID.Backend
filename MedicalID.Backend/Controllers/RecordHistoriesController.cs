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

        // ✅ Patients can view their own records via MedicalID
        [Authorize(Roles = "Patient")]
        [HttpGet("my")]
        public async Task<IActionResult> GetMyRecordHistory()
        {
            var medicalId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(medicalId))
                return Unauthorized("MedicalID missing from token.");

            var records = await _context.RecordHistories
                .Where(r => r.MedicalID == medicalId)
                .Include(r => r.Doctor)
                .Include(r => r.AccessLog)
                .ToListAsync();

            return Ok(records);
        }

        // ✅ Doctors can create record history (MedicalID-based)
        [Authorize(Roles = "Doctor")]
        [HttpPost]
        public async Task<IActionResult> AddRecordHistory([FromBody] RecordHistoryPostDto dto)
        {
            var doctorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (doctorId != dto.DoctorID)
                return Forbid("You cannot use another doctor's ID.");

            var accessGranted = await _context.AccessLogs
                .AnyAsync(a => a.DoctorID == dto.DoctorID &&
                               a.MedicalID == dto.MedicalID &&
                               a.AccessGranted == true);

            if (!accessGranted)
                return StatusCode(403, "Access not granted by the patient.");

            var exists = await _context.RecordHistories.AnyAsync(r => r.LogID == dto.LogID);
            if (exists)
                return Conflict("A record already exists for this log.");

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
            return Ok("Record added.");
        }

        // ✅ Doctors can update record history
        [Authorize(Roles = "Doctor")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRecordHistory(int id, [FromBody] RecordHistoryDTO dto)
        {
            var doctorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // No Include() — avoid Patient navigation completely
            var record = await _context.RecordHistories.FindAsync(id);
            if (record == null) return NotFound();

            if (record.DoctorID != doctorId)
                return Forbid("You cannot update another doctor’s record.");

            // Confirm patient access via MedicalID in AccessLogs
            var accessGranted = await _context.AccessLogs
                .AnyAsync(a => a.DoctorID == doctorId &&
                               a.MedicalID == record.MedicalID &&
                               a.AccessGranted == true);

            if (!accessGranted)
                return StatusCode(403, "Access not granted by the patient.");

            // ✅ Update only allowed fields
            record.DiagnosisNotes = dto.DiagnosisNotes;
            record.TreatmentPlan = dto.TreatmentPlan;
            record.UpdateTime = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Ok("Record updated.");
        }


        // ✅ Doctors can view records by MedicalID
        [Authorize(Roles = "Doctor")]
        [HttpGet("patient/{medicalId}")]
        public async Task<IActionResult> GetPatientRecords(string medicalId)
        {
            var doctorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

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
                .ToListAsync();

            return Ok(records);
        }

        // ✅ Doctors can delete a record
        [Authorize(Roles = "Doctor")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecordHistory(int id)
        {
            var doctorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var record = await _context.RecordHistories.FindAsync(id);
            if (record == null) return NotFound();

            var accessGranted = await _context.AccessLogs
                .AnyAsync(a => a.DoctorID == doctorId &&
                               a.MedicalID == record.MedicalID &&
                               a.AccessGranted == true);

            if (!accessGranted)
                return StatusCode(403, "Access not granted by the patient.");

            _context.RecordHistories.Remove(record);
            await _context.SaveChangesAsync();
            return Ok("Record deleted.");
        }
    }

}



