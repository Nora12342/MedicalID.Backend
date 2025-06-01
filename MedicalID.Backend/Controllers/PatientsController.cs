using MedicalID.Backend.Data;
using MedicalID.Backend.Dtos.Patient;
using MedicalID.Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MedicalID.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PatientsController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Patient")]
        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var patientId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var patient = await _context.Patients
                .Include(p => p.Region)
                .FirstOrDefaultAsync(p => p.PatientID == patientId);
            if (patient == null) return NotFound();
            return Ok(patient);
        }

        [Authorize(Roles = "Patient")]
        [HttpPut("me")]
        public async Task<IActionResult> UpdateMyProfile(PatientUpdateDto updated)
        {
            var patientId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var patient = await _context.Patients.FindAsync(patientId);
            if (patient == null) return NotFound();

            patient.FName = updated.FName;
            patient.LName = updated.LName;
            patient.DateOfBirth = updated.DateOfBirth;
            patient.BloodType = updated.BloodType;
            patient.EmergencyContact = updated.EmergencyContact;
            patient.OrganDonorStatus = updated.OrganDonorStatus;
            patient.RegionID = updated.RegionID;

            await _context.SaveChangesAsync();
            return Ok("Profile updated.");
        }


        [Authorize(Roles = "Doctor")]
        [HttpGet]
        public async Task<IActionResult> GetAllPatients()
        {
            return Ok(await _context.Patients.ToListAsync());
        }

        [Authorize(Roles = "Doctor")]
        [HttpPost]
        public async Task<IActionResult> AddPatient(PatientPostDto dto)
        {
            var newPatient = new Patient
            {
                PatientID = dto.PatientID,
                MedicalID = dto.MedicalID,
                FName = dto.FName,
                LName = dto.LName,
                Gender = dto.Gender,
                DateOfBirth = dto.DateOfBirth,
                BloodType = dto.BloodType,
                EmergencyContact = dto.EmergencyContact,
                OrganDonorStatus = dto.OrganDonorStatus,
                RegionID = dto.RegionID,
                // You can set default values for UserName/PasswordHash or expect another dto for full registration
                UserName = dto.PatientID, // example default
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456") // default password (you can customize this logic)
            };

            _context.Patients.Add(newPatient);
            await _context.SaveChangesAsync();

            return Ok("Patient added successfully.");
        }


        [Authorize(Roles = "Doctor")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(string id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null) return NotFound();
            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            return Ok("Patient deleted.");
        }
    }
}
