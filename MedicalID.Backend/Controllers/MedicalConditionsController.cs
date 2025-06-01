using MedicalID.Backend.Data;
using MedicalID.Backend.Dtos;
using MedicalID.Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicalID.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalConditionsController : ControllerBase
    {
        private readonly MedicalIDContext _context;

        public MedicalConditionsController(MedicalIDContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicalConditionDto>>> GetMedicalConditions()
        {
            var conditions = await _context.MedicalConditions
                .Select(c => new MedicalConditionDto
                {
                    MedConditionID = c.MedConditionID,
                    ConditionName = c.ConditionName,
                    Description = c.Description,
                    DiagnosedDate = (DateTime)c.DiagnosedDate,
                    PatientID = c.PatientID
                }).ToListAsync();

            return Ok(conditions);
        }

        [HttpPost]
        public async Task<ActionResult<MedicalCondition>> PostMedicalCondition(MedicalConditionDto dto)
        {
            var condition = new MedicalCondition
            {
                ConditionName = dto.ConditionName,
                Description = dto.Description,
                DiagnosedDate = dto.DiagnosedDate,
                PatientID = dto.PatientID
            };

            _context.MedicalConditions.Add(condition);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMedicalConditions), new { id = condition.MedConditionID }, condition);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMedicalCondition(int id, MedicalConditionDto dto)
        {
            var condition = await _context.MedicalConditions.FindAsync(id);
            if (condition == null) return NotFound();

            condition.ConditionName = dto.ConditionName;
            condition.Description = dto.Description;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedicalCondition(int id)
        {
            var condition = await _context.MedicalConditions.FindAsync(id);
            if (condition == null) return NotFound();

            _context.MedicalConditions.Remove(condition);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }

}
