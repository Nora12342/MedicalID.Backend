using MedicalID.Backend.Data;
using MedicalID.Backend.Dtos.Specialization;
using MedicalID.Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicalID.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecializationsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SpecializationsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Specializations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SpecializationDTO>>> GetAll()
        {
            var specializations = await _context.Specializations
                .Select(s => new SpecializationDTO
                {
                    SpecializationID = s.SpecializationID,
                    Name = s.Name
                })
                .ToListAsync();

            return Ok(specializations);
        }

        // POST: api/Specializations (Optional Admin-only feature)
        [HttpPost]
        public async Task<ActionResult> AddSpecialization(CreateSpecializationDTO dto)
        {
            var specialization = new Specialization
            {
                Name = dto.Name
            };

            _context.Specializations.Add(specialization);
            await _context.SaveChangesAsync();

            return Ok("Specialization added.");
        }
    }
}
