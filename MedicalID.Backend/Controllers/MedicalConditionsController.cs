using MedicalID.Backend.Data;
using MedicalID.Backend.Models.JoinModels; // For PatientCondition
using MedicalID.Backend.Models; // For Patient and MedicalCondition
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using MedicalID.Backend.Dtos.MedicalCondition;

namespace MedicalID.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Patient")] // Ensure only Patients can access this controller
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
            // Get patient's primary ID from the JWT token's 'sub' claim
            var patientIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(patientIdStr))
                return Unauthorized("Authentication failed: Patient ID missing in token.");

            // Convert the string ID from the token to an integer
            if (!int.TryParse(patientIdStr, out int patientIdAsInt))
            {
                return Unauthorized("Authentication failed: Invalid patient ID format in token.");
            }

            // Find the patient using their integer primary key (ID)
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.ID == patientIdAsInt);
            if (patient == null)
                return Unauthorized("Patient not found."); // This error means no patient with that ID was found

            // Retrieve all conditions linked to this patient
            var result = await _context.PatientConditions
                .Where(pc => pc.PatientID == patient.ID)
                .Include(pc => pc.Condition) // Eagerly load the related MedicalCondition details
                .Select(pc => new MedicalConditionDto // Map to MedicalConditionDto for display
                {
                    ConditionID = pc.Condition.ConditionID,
                    ConditionName = pc.Condition.ConditionName,
                    Description = pc.Condition.Description,
                    DiagnosedDate = pc.Condition.DiagnosedDate,
                    Note = pc.Condition.Note // 'Note' comes from the MedicalCondition entity, not PatientCondition
                })
                .ToListAsync();

            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> AddCondition([FromBody] PatientConditionPostDto dto)
        {
            // Get patient's primary ID from the JWT token's 'sub' claim
            var patientIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(patientIdStr))
                return Unauthorized("Authentication failed: Patient ID missing in token.");

            // Convert the string ID from the token to an integer
            if (!int.TryParse(patientIdStr, out int patientIdAsInt))
            {
                return Unauthorized("Authentication failed: Invalid patient ID format in token.");
            }

            // Find the patient using their integer primary key (ID)
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.ID == patientIdAsInt);
            if (patient == null)
                return Unauthorized("Patient not found.");

            // Check if the medical condition (by name) already exists in the general MedicalConditions table
            var medicalConditionToLink = await _context.MedicalConditions
                .FirstOrDefaultAsync(mc => mc.ConditionName == dto.ConditionName);

            if (medicalConditionToLink == null)
            {
                // If condition doesn't exist, create a new entry in the MedicalConditions table
                medicalConditionToLink = new MedicalCondition
                {
                    ConditionName = dto.ConditionName,
                    Description = dto.Description,
                    DiagnosedDate = dto.DiagnosedDate,
                    Note = dto.Note // The 'Note' from the DTO is stored with the general MedicalCondition definition
                };
                _context.MedicalConditions.Add(medicalConditionToLink);
                await _context.SaveChangesAsync(); // Save to get the ConditionID for linking
            }
            // else: If it exists, we link to the existing one. No update necessary here unless specified.

            // Now, check if this specific patient already has this condition linked
            var patientConditionExists = await _context.PatientConditions
                .AnyAsync(pc => pc.PatientID == patient.ID && pc.ConditionID == medicalConditionToLink.ConditionID);

            if (patientConditionExists)
            {
                return BadRequest("This condition is already linked to this patient.");
            }

            // Create the link in the PatientConditions join table
            var patientCondition = new PatientCondition
            {
                PatientID = patient.ID, // Use the patient's integer primary key
                ConditionID = medicalConditionToLink.ConditionID,
                // Note is NOT included here, as the PatientConditions DB table does not have a 'Note' column
            };

            _context.PatientConditions.Add(patientCondition);
            await _context.SaveChangesAsync(); // Save the new link

            // Return success response. You might return the full DTO or just a success message.
            // Using CreatedAtAction to point to the GetMyConditions method.
            return CreatedAtAction(nameof(GetMyConditions), new { id = medicalConditionToLink.ConditionID }, medicalConditionToLink);
        }

        [HttpDelete("{conditionId}")]
        public async Task<IActionResult> DeleteCondition(int conditionId)
        {
            // Get patient's primary ID from the JWT token's 'sub' claim
            var patientIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(patientIdStr))
                return Unauthorized("Authentication failed: Patient ID missing in token.");

            // Convert the string ID from the token to an integer
            if (!int.TryParse(patientIdStr, out int patientIdAsInt))
            {
                return Unauthorized("Authentication failed: Invalid patient ID format in token.");
            }

            // Find the patient using their integer primary key (ID)
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.ID == patientIdAsInt);
            if (patient == null)
                return Unauthorized("Patient not found.");

            // Find the specific PatientCondition record using the composite key (PatientID, ConditionID)
            var record = await _context.PatientConditions.FindAsync(patient.ID, conditionId);
            if (record == null)
                return NotFound("Condition not found for this patient.");

            _context.PatientConditions.Remove(record);
            await _context.SaveChangesAsync();

            return Ok("Condition removed successfully.");
        }
    }
}