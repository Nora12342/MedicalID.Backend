using MedicalID.Backend.Data;
using MedicalID.Backend.Dtos;
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
        private readonly MedicalIDContext _context;

        public RegionsController(MedicalIDContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RegionGetDto>>> GetRegions()
        {
            var regions = await _context.Regions
                .Select(r => new RegionGetDto
                {
                    RegionID = r.RegionID,
                    Name = r.Name
                }).ToListAsync();

            return Ok(regions);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRegion(int id, RegionGetDto dto)
        {
            var region = await _context.Regions.FindAsync(id);
            if (region == null) return NotFound();

            region.Name = dto.Name;
            // map other properties if exist

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegion(int id)
        {
            var region = await _context.Regions.FindAsync(id);
            if (region == null) return NotFound();

            _context.Regions.Remove(region);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }


}
