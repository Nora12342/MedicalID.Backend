using MedicalID.Backend.Data;
using MedicalID.Backend.Dtos;
using MedicalID.Backend.Dtos.Medication;
using MedicalID.Backend.Models;
using MedicalID.Backend.Models.JoinModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MedicalID.Backend.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Patient")]
    public class MedicationsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MedicationsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicationDto>>> GetMyMedications()
        {
            var nationalId = User.FindFirstValue("sub");
            if (string.IsNullOrWhiteSpace(nationalId))
                return Unauthorized("Token missing sub claim.");

            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.PatientID == nationalId);
            if (patient == null)
                return Unauthorized("Patient not found.");

            var result = await _context.Medications
                .Where(m => m.PatientID == patient.ID)
                .Select(m => new MedicationDto
                {
                    MedicationID = m.MedicationID,
                    MedicationName = m.MedicationName,
                    Dosage = m.Dosage,
                    Frequency = m.Frequency,
                    PrescribedDate = m.PrescribedDate
                })
                .ToListAsync();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddMedication([FromBody] MedicationPostDto dto)
        {
            var nationalId = User.FindFirstValue("sub");
            if (string.IsNullOrWhiteSpace(nationalId))
                return Unauthorized("Token missing sub claim.");

            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.PatientID == nationalId);
            if (patient == null)
                return Unauthorized("Patient not found.");

            var medication = new Medication
            {
                MedicationName = dto.MedicationName,
                Dosage = dto.Dosage,
                Frequency = dto.Frequency,
                PrescribedDate = dto.PrescribedDate,
                PatientID = patient.ID
            };

            _context.Medications.Add(medication);
            await _context.SaveChangesAsync();

            return Ok("Medication added.");
        }
    }

}

