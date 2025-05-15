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
    }


}
