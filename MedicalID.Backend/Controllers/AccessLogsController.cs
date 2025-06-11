using MedicalID.Backend.Data;
using MedicalID.Backend.Dtos.Accesslog;
using MedicalID.Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MedicalID.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccessLogsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AccessLogsController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ Get All Access Logs (for admin/debugging or testing)
        [Authorize(Roles = "Doctor,Patient")]
        [HttpGet]
        public async Task<IActionResult> GetAllAccessLogs()
        {
            var logs = await _context.AccessLogs
                .Select(log => new AccessLogDto
                {
                    LogID = log.LogID,
                    DoctorID = log.DoctorID,
                    MedicalID = log.MedicalID, // ✅ بدل PatientID
                    AccessTime = log.AccessTime,
                    Purpose = log.Purpose,
                    AccessStatus = log.AccessStatus,
                    AccessGranted = log.AccessGranted
                })
                .ToListAsync();

            return Ok(logs);
        }


        // ✅ Doctor: Request access to patient
        [Authorize(Roles = "Doctor")]
        [HttpPost]
        public async Task<IActionResult> RequestAccess([FromBody] AccessLogPostDto dto)
        {
            var doctorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (doctorId == null || string.IsNullOrWhiteSpace(dto.MedicalID))
                return BadRequest("Doctor or MedicalID is invalid.");

            var log = new AccessLog
            {
                DoctorID = doctorId,
                MedicalID = dto.MedicalID,
                AccessTime = DateTime.UtcNow,
                Purpose = dto.Purpose,
                AccessStatus = "Pending",
                AccessGranted = false
            };

            _context.AccessLogs.Add(log);
            await _context.SaveChangesAsync();

            return Ok("Access request sent.");
        }


        //✅ Patient: Approve or reject access
        [Authorize(Roles = "Patient")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccessLog(int id, [FromBody] AccessLogUpdateDTO dto)
        {
            var patientIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(patientIdStr, out int patientId))
                return Unauthorized("Invalid patient ID in token.");

            // ❗ نجيب MedicalID من قاعدة البيانات للمريض ده
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.ID == patientId);
            if (patient == null)
                return NotFound("Patient not found.");

            var medicalId = patient.MedicalID;

            // ✅ نتأكد إن الـ AccessLog ده يخص المريض فعلاً
            var log = await _context.AccessLogs
                .FirstOrDefaultAsync(a => a.LogID == id && a.MedicalID == medicalId);

            if (log == null)
                return StatusCode(403, "Access log not found or not owned by this patient.");

            log.AccessGranted = dto.AccessGranted;
            log.AccessStatus = dto.AccessStatus ?? (dto.AccessGranted ? "Approved" : "Rejected");

            await _context.SaveChangesAsync();

            return Ok("Access log updated successfully.");
        }





    }

}
