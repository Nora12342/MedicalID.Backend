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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AllergyDto>>> GetMyAllergies()
        {
            var nationalId = User.FindFirstValue("sub");
            if (string.IsNullOrWhiteSpace(nationalId))
                return Unauthorized("Token missing sub claim.");

            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.PatientID == nationalId);
            if (patient == null)
                return Unauthorized("Patient not found.");

            var result = await _context.PatientAllergies
                .Where(pa => pa.PatientID == patient.ID)
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

        [HttpPost]
        public async Task<IActionResult> AddAllergy([FromBody] PatientAllergyPostDto dto)
        {
            var nationalId = User.FindFirstValue("sub");
            if (string.IsNullOrWhiteSpace(nationalId))
                return Unauthorized("Token missing sub claim.");

            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.PatientID == nationalId);
            if (patient == null)
                return Unauthorized("Patient not found.");

            var exists = await _context.PatientAllergies
                .AnyAsync(pa => pa.PatientID == patient.ID && pa.AllergyID == dto.AllergyID);

            if (exists)
                return BadRequest("This allergy is already added.");

            var patientAllergy = new PatientAllergy
            {
                PatientID = patient.ID,
                AllergyID = dto.AllergyID,
                Note = dto.Note
            };

            _context.PatientAllergies.Add(patientAllergy);
            await _context.SaveChangesAsync();

            return Ok("Allergy added successfully.");
        }

        [HttpDelete("{allergyId}")]
        public async Task<IActionResult> DeleteAllergy(int allergyId)
        {
            var nationalId = User.FindFirstValue("sub");
            if (string.IsNullOrWhiteSpace(nationalId))
                return Unauthorized("Token missing sub claim.");

            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.PatientID == nationalId);
            if (patient == null)
                return Unauthorized("Patient not found.");

            var record = await _context.PatientAllergies.FindAsync(patient.ID, allergyId);
            if (record == null)
                return NotFound();

            _context.PatientAllergies.Remove(record);
            await _context.SaveChangesAsync();

            return Ok("Allergy removed.");
        }
    }

}
