using MedicalID.Backend.Data;
using MedicalID.Backend.Dtos.Doctor;
using MedicalID.Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MedicalID.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Doctor")]
    public class DoctorsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DoctorsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var doctorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var doctor = await _context.Doctors
                .Include(d => d.Specialization)
                .Include(d => d.Region)
                .FirstOrDefaultAsync(d => d.DoctorID == doctorId);

            if (doctor == null) return NotFound();

            var dto = new DoctorDto
            {
                DoctorID = doctor.DoctorID,
                FName = doctor.FName,
                LName = doctor.LName,
                SpecializationName = doctor.Specialization?.Name,
                RegionName = doctor.Region?.Name
            };

            return Ok(dto);
        }

        [HttpPut("me")]
        public async Task<IActionResult> UpdateMyProfile(DoctorUpdateDto updated)
        {
            var doctorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var doctor = await _context.Doctors.FindAsync(doctorId);
            if (doctor == null) return NotFound();

            doctor.FName = updated.FName;
            doctor.LName = updated.LName;
            doctor.SpecializationID = updated.SpecializationID;
            doctor.RegionID = updated.RegionID;
            await _context.SaveChangesAsync();
            return Ok("Profile updated.");
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetDoctors([FromQuery] string? specialization)
        {
            var query = _context.Doctors
                .Include(d => d.Specialization)
                .Include(d => d.Region)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(specialization))
            {
                query = query.Where(d => d.Specialization.Name == specialization);
            }

            var doctors = await query.Select(d => new DoctorDto
            {
                DoctorID = d.DoctorID,
                FName = d.FName,
                LName = d.LName,
                SpecializationName = d.Specialization.Name,
                RegionName = d.Region.Name
            }).ToListAsync();

            return Ok(doctors);
        }

        [HttpPost]
        public async Task<IActionResult> AddDoctor(DoctorPostDto dto)
        {
            if (await _context.Doctors.AnyAsync(d => d.UserName == dto.UserName))
                return BadRequest("Username already exists for a doctor.");

            var doctor = new Doctor
            {
                DoctorID = dto.DoctorID,
                FName = dto.FName,
                LName = dto.LName,
                SpecializationID = dto.SpecializationID,
                UserName = dto.UserName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.PasswordHash),
                Email = dto.Email,
                Phone = dto.Phone,
                RegionID = dto.RegionID
            };

            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            return Ok("Doctor added successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctor(string id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null) return NotFound();
            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();
            return Ok("Doctor deleted.");
        }
    }
}
