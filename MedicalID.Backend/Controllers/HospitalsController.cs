using MedicalID.Backend.Data;
using MedicalID.Backend.Dtos.Hospitals;
using MedicalID.Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicalID.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HospitalsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public HospitalsController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Doctor,Patient")]
        [HttpGet]
        public async Task<IActionResult> GetHospitals()
        {
            var hospitals = await _context.Hospitals
                .Include(h => h.Region)
                .ThenInclude(r => r.City)
                .ToListAsync();

            return Ok(hospitals);
        }

        [Authorize(Roles = "Doctor")]
        [HttpPost]
        public async Task<IActionResult> AddHospital([FromBody] HospitalPostDto dto)
        {
            var regionExists = await _context.Regions.AnyAsync(r => r.RegionID == dto.RegionID);
            if (!regionExists)
                return BadRequest("Invalid region ID.");

            var hospital = new Hospital
            {
                HospitalName = dto.HospitalName,
                Type = dto.Type,
                ContactInformation = dto.ContactInformation,
                RegionID = dto.RegionID
            };

            _context.Hospitals.Add(hospital);
            await _context.SaveChangesAsync();

            return Ok("Hospital added.");
        }

        [Authorize(Roles = "Doctor")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHospital(int id)
        {
            var hospital = await _context.Hospitals.FindAsync(id);
            if (hospital == null) return NotFound();
            _context.Hospitals.Remove(hospital);
            await _context.SaveChangesAsync();
            return Ok("Hospital deleted.");
        }
    }

}
