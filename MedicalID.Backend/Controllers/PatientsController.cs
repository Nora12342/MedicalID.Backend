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
    }

}
