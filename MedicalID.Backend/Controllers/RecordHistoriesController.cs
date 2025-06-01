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

        // ✅ Patients can view their own record history
        [Authorize(Roles = "Patient")]
        [HttpGet("my")]
        public async Task<IActionResult> GetMyRecordHistory()
        {
            var patientIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(patientIdStr, out int patientId))
                return Unauthorized("Invalid patient ID in token.");

            var records = await _context.RecordHistories
                .Where(r => r.PatientID == patientId)
                .Include(r => r.Doctor)
                .Include(r => r.AccessLog)
                .ToListAsync();

            return Ok(records);
        }

        // ✅ Doctors can create record history if access is granted
        [Authorize(Roles = "Doctor")]
        [HttpPost]
        public async Task<IActionResult> AddRecordHistory([FromBody] RecordHistoryPostDto dto)
        {
            var doctorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (doctorId != dto.DoctorID)
                return Forbid("You cannot use another doctor's ID.");

            var accessGranted = await _context.AccessLogs
                .AnyAsync(a => a.DoctorID == dto.DoctorID &&
                               a.PatientID == dto.PatientID &&
                               a.AccessGranted == true);

            if (!accessGranted)
                return StatusCode(403, "Access not granted by the patient.");

            var exists = await _context.RecordHistories.AnyAsync(r => r.LogID == dto.LogID);
            if (exists)
                return Conflict("A record already exists for this log.");

            var newRecord = new RecordHistory
            {
                PatientID = dto.PatientID,
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

        // ✅ Doctors can update record history if access is granted
        [Authorize(Roles = "Doctor")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRecordHistory(int id, [FromBody] RecordHistoryDTO dto)
        {
            var doctorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var record = await _context.RecordHistories.FindAsync(id);
            if (record == null) return NotFound();

            if (record.DoctorID != doctorId)
                return Forbid("You cannot update another doctor’s record.");

            var accessGranted = await _context.AccessLogs
                .AnyAsync(a => a.DoctorID == doctorId &&
                               a.PatientID == record.PatientID &&
                               a.AccessGranted == true);

            if (!accessGranted)
                return StatusCode(403, "Access not granted by the patient.");

            record.DiagnosisNotes = dto.DiagnosisNotes;
            record.TreatmentPlan = dto.TreatmentPlan;
            record.UpdateTime = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Ok("Record updated.");
        }

        // ✅ Doctors can view patient records if access is granted
        [Authorize(Roles = "Doctor")]
        [HttpGet("patient/{id}")]
        public async Task<IActionResult> GetPatientRecords(int id) // <-- FIX: id is int
        {
            var doctorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var accessGranted = await _context.AccessLogs
                .AnyAsync(a => a.DoctorID == doctorId &&
                               a.PatientID == id &&
                               a.AccessGranted == true);

            if (!accessGranted)
                return StatusCode(403, "Access not granted by the patient.");

            var records = await _context.RecordHistories
                .Where(r => r.PatientID == id)
                .Include(r => r.AccessLog)
                .Include(r => r.Doctor)
                .ToListAsync();

            return Ok(records);
        }

        // ✅ Doctors can delete record history if access is granted
        [Authorize(Roles = "Doctor")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecordHistory(int id)
        {
            var record = await _context.RecordHistories.FindAsync(id);
            if (record == null) return NotFound();

            var accessGranted = await _context.AccessLogs
                .AnyAsync(a => a.DoctorID == record.DoctorID &&
                               a.PatientID == record.PatientID &&
                               a.AccessGranted == true);

            if (!accessGranted)
                return StatusCode(403, "Access not granted by the patient.");

            _context.RecordHistories.Remove(record);
            await _context.SaveChangesAsync();
            return Ok("Record deleted.");
        }
    }

}



