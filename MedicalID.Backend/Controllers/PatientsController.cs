using MedicalID.Backend.Data;
using MedicalID.Backend.Dtos.Patient;
using MedicalID.Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicalID.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly MedicalIDContext _context;

        public PatientsController(MedicalIDContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientDto>>> GetPatients()
        {
            var patients = await _context.Patients
                .Include(p => p.Region)
                .Select(p => new PatientDto
                {
                    PatientID = p.PatientID,
                    FName = p.FName,
                    LName = p.LName,
                    DateOfBirth = p.DateOfBirth,
                    Gender = p.Gender,
                    //RegionName = p.Region.RegionName
                }).ToListAsync();

            return Ok(patients);
        }

        [HttpPost]
        public async Task<ActionResult<Patient>> PostPatient(PatientPostDto dto)
        {
            var patient = new Patient
            {
                FName = dto.FName,
                LName = dto.LName,
                DateOfBirth = dto.DateOfBirth,
                Gender = dto.Gender,
                RegionID = dto.RegionID
            };

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPatients), new { id = patient.PatientID }, patient);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(int id, PatientPostDto dto)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null) return NotFound();

            patient.FName = dto.FName;
            patient.LName = dto.LName;
            patient.Gender = dto.Gender;
            patient.MedicalID = dto.MedicalID;
            // map other properties as needed

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null) return NotFound();

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }

}
