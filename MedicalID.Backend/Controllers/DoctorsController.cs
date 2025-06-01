using MedicalID.Backend.Data;
using MedicalID.Backend.Dtos.Doctor;
using MedicalID.Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicalID.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly MedicalIDContext _context;

        public DoctorsController(MedicalIDContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DoctorDto>>> GetDoctors()
        {
            var doctors = await _context.Doctors
                .Include(d => d.Region)
                .Select(d => new DoctorDto
                {
                    DoctorID = d.DoctorID,
                    FName = d.FName,
                    LName = d.LName,
                    Specialization = d.Specialization,
                    Email = d.Email,
                    RegionID = d.RegionID
                }).ToListAsync();

            return Ok(doctors);
        }

        [HttpPost]
        public async Task<ActionResult<Doctor>> PostDoctor(DoctorPostDto dto)
        {
            var doctor = new Doctor
            {
                DoctorID = dto.DoctorID,
                FName = dto.FName,
                LName = dto.LName,
                Specialization = dto.Specialization,
                Email = dto.Email,
                RegionID = dto.RegionID
            };

            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDoctors), new { id = doctor.DoctorID }, doctor);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDoctor(int id, DoctorPostDto dto)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null) return NotFound();

            doctor.FName = dto.FName;
            doctor.LName = dto.LName;
            doctor.Email = dto.Email;
            doctor.Specialization = dto.Specialization;
            doctor.Phone = dto.Phone;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null) return NotFound();

            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }

}
