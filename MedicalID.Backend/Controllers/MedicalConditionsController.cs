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
    public class MedicalConditionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MedicalConditionsController(AppDbContext context)
        {
            _context = context;
        }

        // 🧑‍🦰 Patient: View their conditions
        [Authorize(Roles = "Patient")]
        [HttpGet("my")]
        public async Task<IActionResult> GetMyConditions()
        {
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdStr, out int patientId))
                return Unauthorized("Invalid patient ID in token.");

            var conditions = await _context.PatientConditions
                .Where(pc => pc.PatientID == patientId)
                .Include(pc => pc.Condition)
                .Select(pc => new MedicalConditionDto
                {
                    ConditionID = pc.ConditionID,
                    ConditionName = pc.Condition.ConditionName,
                    Description = pc.Condition.Description,
                    DiagnosedDate = pc.Condition.DiagnosedDate
                    // Removed Note ✅
                })
                .ToListAsync();

            return Ok(conditions);
        }

        // 👨‍⚕️ Doctor: Assign condition to patient
        [Authorize(Roles = "Doctor")]
        [HttpPost]
        public async Task<IActionResult> AddCondition([FromBody] PatientConditionPostDto dto)
        {
            var newEntry = new PatientCondition
            {
                PatientID = dto.PatientID,
                ConditionID = dto.ConditionID
                // Removed Note ✅
            };

            _context.PatientConditions.Add(newEntry);
            await _context.SaveChangesAsync();
            return Ok("Condition added successfully.");
        }

        // 👨‍⚕️ Doctor: Remove assigned condition
        [Authorize(Roles = "Doctor")]
        [HttpDelete("{patientId:int}/{conditionId}")]
        public async Task<IActionResult> DeleteCondition(int patientId, int conditionId)
        {
            var entry = await _context.PatientConditions.FindAsync(patientId, conditionId);
            if (entry == null) return NotFound();

            _context.PatientConditions.Remove(entry);
            await _context.SaveChangesAsync();

            return Ok("Condition removed successfully.");
        }
    }

}
