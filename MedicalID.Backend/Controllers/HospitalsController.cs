using MedicalID.Backend.Data;
using MedicalID.Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace MedicalID.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HospitalsController : ControllerBase
    {
        private readonly MedicalIDContext _context;

        public HospitalsController(MedicalIDContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Hospital>>> GetHospitals()
        {
            return await _context.Hospitals.ToListAsync();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHospital(int id, Hospital dto)
        {
            var hospital = await _context.Hospitals.FindAsync(id);
            if (hospital == null) return NotFound();

            hospital.Name = dto.Name;
            hospital.Region = dto.Region;
            hospital.RegionID = dto.RegionID;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHospital(int id)
        {
            var hospital = await _context.Hospitals.FindAsync(id);
            if (hospital == null) return NotFound();

            _context.Hospitals.Remove(hospital);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
