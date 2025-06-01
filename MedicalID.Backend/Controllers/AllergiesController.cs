using MedicalID.Backend.Data;
using MedicalID.Backend.Dtos;
using MedicalID.Backend.Dtos.Allergy;
using MedicalID.Backend.Models.JoinModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MedicalID.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Patient")]
    public class AllergiesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AllergiesController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ GET: Return allergy list for current patient
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AllergyDto>>> GetMyAllergies()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdString, out int userId))
                return Unauthorized("Invalid patient ID in token.");

            var result = await _context.PatientAllergies
                .Where(pa => pa.PatientID == userId)
                .Include(pa => pa.Allergy)
                .Select(pa => new AllergyDto
                {
                    AllergyID = pa.AllergyID,
                    Allergen = pa.Allergy.Allergen,
                    Severity = pa.Allergy.Severity,
                    Reaction = pa.Allergy.Reaction
                })
                .ToListAsync();

            return Ok(result);
        }

        // ✅ POST: Link an existing allergy to current patient
        [HttpPost]
        public async Task<IActionResult> AddAllergy([FromBody] int allergyId)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdString, out int userId))
                return Unauthorized("Invalid patient ID in token.");

            var exists = await _context.PatientAllergies
                .AnyAsync(pa => pa.PatientID == userId && pa.AllergyID == allergyId);

            if (exists)
                return BadRequest("This allergy is already added.");

            var patientAllergy = new PatientAllergy
            {
                PatientID = userId,
                AllergyID = allergyId
            };

            _context.PatientAllergies.Add(patientAllergy);
            await _context.SaveChangesAsync();

            return Ok("Allergy added successfully.");
        }

        // ✅ DELETE: Remove allergy by ID
        [HttpDelete("{allergyId}")]
        public async Task<IActionResult> DeleteAllergy(int allergyId)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdString, out int userId))
                return Unauthorized("Invalid patient ID in token.");

            var record = await _context.PatientAllergies.FindAsync(userId, allergyId);

            if (record == null)
                return NotFound();

            _context.PatientAllergies.Remove(record);
            await _context.SaveChangesAsync();

            return Ok("Allergy removed.");
        }
    }

}
