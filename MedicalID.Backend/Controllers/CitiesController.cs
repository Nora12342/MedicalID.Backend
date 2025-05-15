using MedicalID.Backend.Data;
using MedicalID.Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace MedicalID.Backend.Controllers
{
    public class CitiesController : ControllerBase
    {
        private readonly MedicalIDContext _context;

        public CitiesController(MedicalIDContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<City>>> GetCities()
        {
            return await _context.Cities.ToListAsync();
        }
    }
}
