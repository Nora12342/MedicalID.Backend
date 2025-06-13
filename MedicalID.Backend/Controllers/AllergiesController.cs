using MedicalID.Backend.Data;
using MedicalID.Backend.Dtos;
// Assuming your DTOs are in a subfolder like Allergy, adjust namespace if different
using MedicalID.Backend.Dtos.Allergy;
using MedicalID.Backend.Models.JoinModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims; // Make sure this is present

namespace MedicalID.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Patient")] // Keep this attribute enabled
    public class AllergiesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AllergiesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientAllergyDto>>> GetMyAllergies()
        {
            var patientIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get the 'sub' claim (e.g., "1")
            if (string.IsNullOrWhiteSpace(patientIdStr))
                return Unauthorized("Authentication failed: Patient ID missing in token.");

            if (!int.TryParse(patientIdStr, out int patientIdAsInt)) // Convert string ID to int
            {
                return Unauthorized("Authentication failed: Invalid patient ID format in token.");
            }

            // Find the patient by their internal ID (int primary key)
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.ID == patientIdAsInt);
            if (patient == null)
                return Unauthorized("Patient not found.");

            var result = await _context.PatientAllergies
                .Where(pa => pa.PatientID == patient.ID) // Use patient.ID for the join
                .Include(pa => pa.Allergy) // Include the Allergy navigation property
                .Select(pa => new PatientAllergyDto // Map to PatientAllergyDto
                {
                    AllergyID = pa.AllergyID,
                    Allergen = pa.Allergy.Allergen, // Access Allergen from the related Allergy entity
                    Severity = pa.Allergy.Severity, // Access Severity from the related Allergy entity
                    Reaction = pa.Allergy.Reaction,
                    
                    
                })
                .ToListAsync();

            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> AddAllergy([FromBody] PatientAllergyPostDto dto) // This DTO should contain Allergen (string) and Note
        {
            var patientIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get the 'sub' claim
            if (string.IsNullOrWhiteSpace(patientIdStr))
                return Unauthorized("Authentication failed: Patient ID missing in token.");

            if (!int.TryParse(patientIdStr, out int patientIdAsInt)) // Convert string ID to int
            {
                return Unauthorized("Authentication failed: Invalid patient ID format in token.");
            }

            // Find the patient by their internal ID (int primary key)
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.ID == patientIdAsInt);
            if (patient == null)
                return Unauthorized("Patient not found.");

            // STEP 1: Find the Allergy ID from the Allergen name provided in the DTO
            var allergy = await _context.Allergies.FirstOrDefaultAsync(a => a.Allergen == dto.Allergen);
            if (allergy == null)
            {
                // Return a BadRequest if the allergen name doesn't exist in your Allergies table
                return BadRequest($"Allergen '{dto.Allergen}' not found in the system. Please ensure the allergen exists.");
            }

            // STEP 2: Check if this patient already has this specific allergy using the found AllergyID
            var exists = await _context.PatientAllergies
                .AnyAsync(pa => pa.PatientID == patient.ID && pa.AllergyID == allergy.AllergyID);

            if (exists)
                return BadRequest("This allergy is already added for this patient.");

            // STEP 3: Create and add the new PatientAllergy record
            var patientAllergy = new PatientAllergy
            {
                PatientID = patient.ID, // Use the patient's internal ID
                AllergyID = allergy.AllergyID // Use the ID from the found allergy
                
            };

            _context.PatientAllergies.Add(patientAllergy);
            await _context.SaveChangesAsync();

            return Ok("Allergy added successfully.");
        }



        [HttpDelete("{allergyId}")]
        public async Task<IActionResult> DeleteAllergy(int allergyId)
        {
            var patientIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get the 'sub' claim
            if (string.IsNullOrWhiteSpace(patientIdStr))
                return Unauthorized("Authentication failed: Patient ID missing in token.");

            if (!int.TryParse(patientIdStr, out int patientIdAsInt)) // Convert string ID to int
            {
                return Unauthorized("Authentication failed: Invalid patient ID format in token.");
            }

            // Find the patient by their internal ID (int primary key)
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.ID == patientIdAsInt);
            if (patient == null)
                return Unauthorized("Patient not found.");

            // Find the record using patient.ID and the provided allergyId
            // Assuming FindAsync works on a composite key (PatientID, AllergyID)
            var record = await _context.PatientAllergies.FindAsync(patient.ID, allergyId);
            if (record == null)
                return NotFound("Patient allergy record not found for this patient and allergy ID.");

            _context.PatientAllergies.Remove(record);
            await _context.SaveChangesAsync();

            return Ok("Allergy removed.");
        }
    }
}