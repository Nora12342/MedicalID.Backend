using MedicalID.Backend.Data;
using MedicalID.Backend.Dtos.AskDoctor;
using MedicalID.Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MedicalID.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AskDoctorController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AskDoctorController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Patient")]
        [HttpGet("patient")]
        public async Task<IActionResult> GetDoctorMessages()
        {
            var doctorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var messages = await _context.AskDoctors
                //.Where(m => m.DoctorID == doctorId)
                .Include(m => m.Patient)
                .Include(m => m.Doctor)
                .Select(m => new AskDoctorDto
                {
                    MessageID = m.MessageID,
                    Question = m.MessageContent,
                    Response = m.ResponseContent,
                    SentAt = m.SentAt,
                    RepliedAt = m.RepliedAt,
                    DoctorName = $"{m.Doctor.FName} {m.Doctor.LName}",
                    PatientName = $"{m.Patient.FName} {m.Patient.LName}",
                    UpiRef = m.UpiRef
                })
                .ToListAsync();

            return Ok(messages);
        }

        [Authorize(Roles = "Doctor")]
        [HttpGet("doctor")]
        public async Task<IActionResult> GetMyQuestions()
        {
            //var patientIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //if (!int.TryParse(patientIdStr, out int patientId))
            //    return Unauthorized("Invalid patient ID in token.");

            var messages = await _context.AskDoctors
                //.Where(m => m.PatientID == patientId)
                .Include(m => m.Doctor)
                .Include(m => m.Patient)
                .Select(m => new AskDoctorDto
                {
                    MessageID = m.MessageID,
                    Question = m.MessageContent,
                    Response = m.ResponseContent,
                    SentAt = m.SentAt,
                    RepliedAt = DateTime.Now,
                    DoctorName = $"{m.Doctor.FName} {m.Doctor.LName}",
                    PatientName = $"{m.Patient.FName} {m.Patient.LName}",
                    UpiRef = m.UpiRef,
                    Subject = m.Subject,
                    
                    
                  
                })
                .ToListAsync();

            return Ok(messages);
        }

        [Authorize(Roles = "Patient")]
        [HttpPost]
        public async Task<IActionResult> AskDoctor([FromBody] AskDoctorPostDto dto)
        {
            //var patientIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value??"1";
            //if (!int.TryParse(patientIdStr, out int patientId))
            //    return Unauthorized("Invalid patient ID in token.");

            //var PatientId = _context.Find(dto.PatientID);

            var patientId = 1;

            if (patientId != dto.PatientID)
                return Forbid();

            var doctorExists = await _context.Doctors.AnyAsync(d => d.DoctorID == dto.DoctorID);
            if (!doctorExists) return BadRequest("Doctor not found.");

            if (!dto.IsPaid || dto.AmountPaid <= 0)
                return BadRequest("Payment is required before submitting a question.");

            var message = new AskDoctor
            {
                DoctorID = dto.DoctorID,
                PatientID = dto.PatientID,
                MessageContent = dto.Question,
                SentAt = DateTime.UtcNow,
                IsRead = false,
                IsPaid = dto.IsPaid,
                AmountPaid = dto.AmountPaid,
                UpiRef = dto.UpiRef,
                Subject = dto.Subject
            };

            _context.AskDoctors.Add(message);
            await _context.SaveChangesAsync();

            return Ok("Question sent to doctor.");
        }

        [Authorize(Roles = "Doctor")]
        [HttpPut("respond/{id}")]
        public async Task<IActionResult> RespondToMessage(int id, [FromBody] AskDoctorResponseDto dto)
        {
            var doctorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var message = await _context.AskDoctors.FindAsync(id);

            if (message == null) return NotFound();
            if (message.DoctorID != doctorId) return Forbid();

            message.ResponseContent = dto.ResponseContent;
            message.IsRead = true;
            message.RepliedAt = DateTime.UtcNow;

            _context.Entry(message).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok("Response submitted.");
        }

        [Authorize(Roles = "Doctor")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var doctorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var message = await _context.AskDoctors.FindAsync(id);

            if (message == null) return NotFound();
            if (message.DoctorID != doctorId) return Forbid();

            _context.AskDoctors.Remove(message);
            await _context.SaveChangesAsync();

            return Ok("Message deleted.");
        }
    }
}


