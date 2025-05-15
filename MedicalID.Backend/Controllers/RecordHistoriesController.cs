using MedicalID.Backend.Data;
using MedicalID.Backend.Dtos.RecordHistory;
using MedicalID.Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace MedicalID.Backend.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RecordHistoryController : ControllerBase
    {
        private readonly MedicalIDContext _context;

        public RecordHistoryController(MedicalIDContext context)
        {
            _context = context;
        }

        // ✅ Allow both Doctors and Patients to view record histories, if needed
        [Authorize(Roles = "Doctor,Patient")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecordHistoryDto>>> GetRecordHistories()
        {
            var records = await _context.RecordHistories
                .Include(r => r.Doctor)
                .Include(r => r.Patient)
                .Select(r => new RecordHistoryDto
                {
                    RecordID = r.RecordID,
                    Description = r.Description,
                    UpdatedAt = r.UpdatedAt,
                    DoctorName = r.Doctor.FName + " " + r.Doctor.LName,
                    PatientName = r.Patient.FName + " " + r.Patient.LName
                }).ToListAsync();

            return Ok(records);
        }

        // ✅ Only Doctors can add record histories
        [Authorize(Roles = "Doctor")]
        [HttpPost]
        public async Task<ActionResult<RecordHistory>> PostRecordHistory(RecordHistoryPostDto dto)
        {
            var record = new RecordHistory
            {
                Description = dto.Description,
                UpdatedAt = dto.UpdatedAt,
                DoctorID = dto.DoctorID,
                PatientID = dto.PatientID
            };

            _context.RecordHistories.Add(record);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRecordHistories), new { id = record.RecordID }, record);
        }
    }
}

