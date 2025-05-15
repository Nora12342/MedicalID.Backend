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
    }
}
