using MedicalID.Backend.Data;
using MedicalID.Backend.Models.JoinModels;
using MedicalID.Backend.Models; 
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
            
            var patientIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(patientIdStr))
                return Unauthorized("Authentication failed: Patient ID missing in token.");

            
            if (!int.TryParse(patientIdStr, out int patientIdAsInt))
            {
                return Unauthorized("Authentication failed: Invalid patient ID format in token.");
            }

            
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.ID == patientIdAsInt);
            if (patient == null)
                return Unauthorized("Patient not found."); 

            
            var result = await _context.PatientConditions
                .Where(pc => pc.PatientID == patient.ID)
                .Include(pc => pc.Condition) 
                .Select(pc => new MedicalConditionDto 
                {
                    ConditionID = pc.Condition.ConditionID,
                    ConditionName = pc.Condition.ConditionName,
                    Description = pc.Condition.Description,
                    DiagnosedDate = pc.Condition.DiagnosedDate,
                    Note = pc.Condition.Note 
                })
                .ToListAsync();

            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> AddCondition([FromBody] PatientConditionPostDto dto)
        {
            
            var patientIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(patientIdStr))
                return Unauthorized("Authentication failed: Patient ID missing in token.");

           
            if (!int.TryParse(patientIdStr, out int patientIdAsInt))
            {
                return Unauthorized("Authentication failed: Invalid patient ID format in token.");
            }

            
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.ID == patientIdAsInt);
            if (patient == null)
                return Unauthorized("Patient not found.");

            
            var medicalConditionToLink = await _context.MedicalConditions
                .FirstOrDefaultAsync(mc => mc.ConditionName == dto.ConditionName);

            if (medicalConditionToLink == null)
            {
                
                medicalConditionToLink = new MedicalCondition
                {
                    ConditionName = dto.ConditionName,
                    Description = dto.Description,
                    DiagnosedDate = dto.DiagnosedDate,
                    Note = dto.Note 
                };
                _context.MedicalConditions.Add(medicalConditionToLink);
                await _context.SaveChangesAsync(); 
            }
           

            
            var patientConditionExists = await _context.PatientConditions
                .AnyAsync(pc => pc.PatientID == patient.ID && pc.ConditionID == medicalConditionToLink.ConditionID);

            if (patientConditionExists)
            {
                return BadRequest("This condition is already linked to this patient.");
            }

            
            var patientCondition = new PatientCondition
            {
                PatientID = patient.ID, 
                ConditionID = medicalConditionToLink.ConditionID,
               
            };

            _context.PatientConditions.Add(patientCondition);
            await _context.SaveChangesAsync(); 

            
            return CreatedAtAction(nameof(GetMyConditions), new { id = medicalConditionToLink.ConditionID }, medicalConditionToLink);
        }

        [HttpDelete("{conditionId}")]
        public async Task<IActionResult> DeleteCondition(int conditionId)
        {
           
            var patientIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(patientIdStr))
                return Unauthorized("Authentication failed: Patient ID missing in token.");

           
            if (!int.TryParse(patientIdStr, out int patientIdAsInt))
            {
                return Unauthorized("Authentication failed: Invalid patient ID format in token.");
            }

            
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.ID == patientIdAsInt);
            if (patient == null)
                return Unauthorized("Patient not found.");

            
            var record = await _context.PatientConditions.FindAsync(patient.ID, conditionId);
            if (record == null)
                return NotFound("Condition not found for this patient.");

            _context.PatientConditions.Remove(record);
            await _context.SaveChangesAsync();

            return Ok("Condition removed successfully.");
        }
    }
}