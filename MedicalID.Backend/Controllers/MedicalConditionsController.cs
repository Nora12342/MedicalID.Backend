using MedicalID.Backend.Data;
using MedicalID.Backend.Models.JoinModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using MedicalID.Backend.Dtos.MedicalCondition;

namespace MedicalID.Backend.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Patient")]
    public class MedicalConditionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MedicalConditionsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicalConditionDto>>> GetMyConditions()
        {
            var nationalId = User.FindFirstValue("sub");
            if (string.IsNullOrWhiteSpace(nationalId))
                return Unauthorized("Token missing sub claim.");

            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.PatientID == nationalId);
            if (patient == null)
                return Unauthorized("Patient not found.");

            var result = await _context.PatientConditions
                .Where(pc => pc.PatientID == patient.ID)
                .Include(pc => pc.Condition)
                .Select(pc => new MedicalConditionDto
                {
                    ConditionID = pc.ConditionID,
                    ConditionName = pc.Condition.ConditionName,
                    Description = pc.Condition.Description,
                    DiagnosedDate = pc.Condition.DiagnosedDate,
                    Note = pc.Note
                })
                .ToListAsync();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddCondition([FromBody] PatientConditionPostDto dto)
        {
            var nationalId = User.FindFirstValue("sub");
            if (string.IsNullOrWhiteSpace(nationalId))
                return Unauthorized("Token missing sub claim.");

            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.PatientID == nationalId);
            if (patient == null)
                return Unauthorized("Patient not found.");

            var exists = await _context.PatientConditions
                .AnyAsync(pc => pc.PatientID == patient.ID && pc.ConditionID == dto.ConditionID);

            if (exists)
                return BadRequest("This condition is already added.");

            var patientCondition = new PatientCondition
            {
                PatientID = patient.ID,
                ConditionID = dto.ConditionID,
                Note = dto.Note
            };

            _context.PatientConditions.Add(patientCondition);
            await _context.SaveChangesAsync();

            return Ok("Condition added.");
        }

        [HttpDelete("{conditionId}")]
        public async Task<IActionResult> DeleteCondition(int conditionId)
        {
            var nationalId = User.FindFirstValue("sub");
            if (string.IsNullOrWhiteSpace(nationalId))
                return Unauthorized("Token missing sub claim.");

            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.PatientID == nationalId);
            if (patient == null)
                return Unauthorized("Patient not found.");

            var record = await _context.PatientConditions.FindAsync(patient.ID, conditionId);
            if (record == null)
                return NotFound();

            _context.PatientConditions.Remove(record);
            await _context.SaveChangesAsync();

            return Ok("Condition removed.");
        }
    }



}
