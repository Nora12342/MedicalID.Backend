using MedicalID.Backend.Data;
using MedicalID.Backend.Dtos.Region;
using MedicalID.Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicalID.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RegionsController(AppDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetRegions()
        {
            // ✅ FIXED: Remove .Include(r => r.Cities) since Region no longer has Cities
            return Ok(await _context.Regions
                .Include(r => r.City) // Optional: include city info for each region
                .ToListAsync());
        }

        [Authorize(Roles = "Doctor")]
        [HttpPost]
        public async Task<IActionResult> CreateRegion([FromBody] RegionPostDto dto)
        {
            var cityExists = await _context.City.AnyAsync(c => c.CityID == dto.CityID);
            if (!cityExists)
                return BadRequest("Invalid city ID.");

            var region = new Region
            {
                Name = dto.Name,
                CityID = dto.CityID
            };

            _context.Regions.Add(region);
            await _context.SaveChangesAsync();

            return Ok("Region created successfully.");
        }

        [Authorize(Roles = "Doctor")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegion(int id)
        {
            var region = await _context.Regions.FindAsync(id);
            if (region == null) return NotFound();
            _context.Regions.Remove(region);
            await _context.SaveChangesAsync();
            return Ok("Region deleted.");
        }
    }

}