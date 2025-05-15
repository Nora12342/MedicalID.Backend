using MedicalID.Backend.Data;
using MedicalID.Backend.Dtos;
using MedicalID.Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace MedicalID.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AllergiesController : ControllerBase
    {
        private readonly MedicalIDContext _context;

        public AllergiesController(MedicalIDContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AllergyDto>>> GetAllergies()
        {
            var allergies = await _context.Allergies
                .Select(a => new AllergyDto
                {
                    Allergen = a.Allergen,
                    Reaction = a.Reaction,
                    Severity = a.Severity,
                    PatientId = a.PatientID
                }).ToListAsync();

            return Ok(allergies);
        }

        [HttpPost]
        public async Task<ActionResult<Allergy>> PostAllergy(AllergyDto dto)
        {
            var allergy = new Allergy
            {
                Allergen = dto.Allergen,
                Reaction = dto.Reaction,
                Severity = dto.Severity,
                PatientID = dto.PatientId
            };

            _context.Allergies.Add(allergy);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAllergies), new { id = allergy.Allergen }, allergy);
        }
    }

}
