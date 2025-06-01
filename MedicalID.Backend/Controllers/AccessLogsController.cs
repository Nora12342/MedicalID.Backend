using MedicalID.Backend.Data;
using MedicalID.Backend.Dtos.Accesslog;
using MedicalID.Backend.Models;
using Microsoft.AspNetCore.Authorization; // ✅ Add this
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicalID.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccessLogController : ControllerBase
    {
        private readonly MedicalIDContext _context;

        public AccessLogController(MedicalIDContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Doctor,Patient")] // ✅ Both roles can see access logs
        public async Task<ActionResult<IEnumerable<AccessLogDto>>> GetAccessLogs()
        {
            var logs = await _context.AccessLogs
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Select(a => new AccessLogDto
                {
                    LogID = a.LogID,
                    AccessTime = a.AccessTime,
                    Purpose = a.Purpose,
                    DoctorName = a.Doctor.FName + " " + a.Doctor.LName,
                    PatientName = a.Patient.FName + " " + a.Patient.LName
                }).ToListAsync();

            return Ok(logs);
        }

        [HttpPost]
        [Authorize(Roles = "Doctor")] // ✅ Only doctors log access
        public async Task<ActionResult<AccessLog>> PostAccessLog(AccessLogPostDto dto)
        {
            var log = new AccessLog
            {
                AccessTime = dto.AccessTime,
                Purpose = dto.Purpose,
                DoctorID = dto.DoctorID,
                PatientID = dto.PatientID
            };

            _context.AccessLogs.Add(log);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAccessLogs), new { id = log.LogID }, log);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccessLog(int id, AccessLogPostDto dto)
        {
            var log = await _context.AccessLogs.FindAsync(id);
            if (log == null) return NotFound();

            log.PatientID = dto.PatientID;
            log.DoctorID = dto.DoctorID;
            log.AccessTime = dto.AccessTime;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccessLog(int id)
        {
            var log = await _context.AccessLogs.FindAsync(id);
            if (log == null) return NotFound();

            _context.AccessLogs.Remove(log);
            await _context.SaveChangesAsync();
            return NoContent();
        }



    }
}
