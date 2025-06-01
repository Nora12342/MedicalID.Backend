
    using MedicalID.Backend.Data;
    using MedicalID.Backend.Models;
    using MedicalID.Backend.Dtos.City;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using global::MedicalID.Backend.Dtos.City;

    namespace MedicalID.Backend.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class CityController : ControllerBase
        {
            private readonly AppDbContext _context;

            public CityController(AppDbContext context)
            {
                _context = context;
            }

            // ✅ GET: Return all cities with their regions
            [Authorize(Roles = "Doctor,Patient")]
            [HttpGet]
            public async Task<IActionResult> GetCities()
            {
                var cities = await _context.City
                    .Include(c => c.Regions)
                    .ToListAsync();

                return Ok(cities);
            }

            // ✅ POST: Add new city + regions using clean DTO
            [Authorize(Roles = "Doctor")]
            [HttpPost]
            public async Task<IActionResult> AddCity([FromBody] CityPostDto dto)
            {
                if (string.IsNullOrWhiteSpace(dto.CityName))
                    return BadRequest("City name is required.");

                var city = new City
                {
                    CityName = dto.CityName,
                    Regions = dto.RegionNames?.Select(name => new Region
                    {
                        Name = name
                    }).ToList()
                };

                _context.City.Add(city);
                await _context.SaveChangesAsync();

                return Ok("City added.");
            }

            // ✅ DELETE: Delete city by ID
            [Authorize(Roles = "Doctor")]
            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteCity(int id)
            {
                var city = await _context.City
                    .Include(c => c.Regions)
                    .FirstOrDefaultAsync(c => c.CityID == id);

                if (city == null)
                    return NotFound("City not found.");

                _context.City.Remove(city);
                await _context.SaveChangesAsync();

                return Ok("City deleted.");
            }
        }
    }

