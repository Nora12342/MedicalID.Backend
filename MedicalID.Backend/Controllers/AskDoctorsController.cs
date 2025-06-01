using MedicalID.Backend.Data;
using MedicalID.Backend.Dtos.AskDoctor;
using MedicalID.Backend.Models;
using Microsoft.AspNetCore.Authorization; // ✅ Add this
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicalID.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AskDoctorController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AskDoctorController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Authorize(Roles = "Patient")] // ✅ Only patients can ask
        public async Task<IActionResult> CreateAskDoctor([FromBody] AskDoctorPostDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var askDoctor = new AskDoctor
            {
                Question = dto.Question,
                DoctorID = dto.DoctorID,
                PatientID = dto.PatientID,
                SentAt = DateTime.UtcNow,
                Response = null,
                RepliedAt = null
            };

            _context.AskDoctors.Add(askDoctor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAskDoctorById), new { id = askDoctor.MessageID }, askDoctor);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Doctor,Patient")] // ✅ Both can read the message
        public async Task<ActionResult<AskDoctorDto>> GetAskDoctorById(int id)
        {
            var ask = await _context.AskDoctors
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(a => a.MessageID == id);

            if (ask == null) return NotFound();

            var result = new AskDoctorDto
            {
                MessageID = ask.MessageID,
                Question = ask.Question,
                Response = ask.Response,
                SentAt = ask.SentAt,
                RepliedAt = ask.RepliedAt,
                DoctorName = ask.Doctor?.FName + " " + ask.Doctor?.LName,
                PatientName = ask.Patient?.FName + " " + ask.Patient?.LName
            };

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAskDoctor(int id, AskDoctorPostDto dto)
        {
            var askDoctor = await _context.AskDoctors.FindAsync(id);
            if (askDoctor == null) return NotFound();

            askDoctor.PatientID = dto.PatientID;
            askDoctor.DoctorID = dto.DoctorID;
            askDoctor.Question = dto.Question;
            askDoctor.Answer = dto.Answer;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAskDoctor(int id)
        {
            var askDoctor = await _context.AskDoctors.FindAsync(id);
            if (askDoctor == null) return NotFound();

            _context.AskDoctors.Remove(askDoctor);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}

