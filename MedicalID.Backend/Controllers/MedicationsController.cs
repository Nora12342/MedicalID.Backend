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
    public class MedicationsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MedicationsController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ Get all medications assigned to the logged-in patient
        [Authorize(Roles = "Patient")]
        [HttpGet("my")]
        public async Task<IActionResult> GetMyMedications()
        {
            var patientIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(patientIdStr, out int patientId))
                return Unauthorized("Invalid patient ID in token.");

            var medications = await _context.PatientMedications
                .Where(pm => pm.PatientID == patientId)
                .Include(pm => pm.Medication)
                .Select(pm => new MedicationDto
                {
                    MedicationID = pm.Medication.MedicationID,
                    MedicationName = pm.Medication.MedicationName,
                    Dosage = pm.Medication.Dosage,
                    Frequency = pm.Medication.Frequency,
                    PrescribedDate = pm.Medication.PrescribedDate
                })
                .ToListAsync();

            return Ok(medications);
        }

        // ✅ Add medication + assign it to a patient (Doctor only)
        [Authorize(Roles = "Doctor")]
        [HttpPost]
        public async Task<IActionResult> AddMedication([FromBody] MedicationPostDto dto)
        {
            var medication = new Medication
            {
                MedicationName = dto.MedicationName,
                Dosage = dto.Dosage,
                Frequency = dto.Frequency,
                PrescribedDate = dto.PrescribedDate
            };

            _context.Medications.Add(medication);
            await _context.SaveChangesAsync();

            // After saving, get the generated MedicationID and assign it
            var patientMedication = new PatientMedication
            {
                PatientID = dto.PatientID,
                MedicationID = medication.MedicationID
            };

            _context.PatientMedications.Add(patientMedication);
            await _context.SaveChangesAsync();

            return Ok("Medication added and assigned.");
        }

        [Authorize(Roles = "Doctor")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMedication(int id, [FromBody] MedicationUpdateDTO dto)
        {
            var medication = await _context.Medications.FindAsync(id);
            if (medication == null) return NotFound("Medication not found.");

            medication.MedicationName = dto.MedicationName;
            medication.Dosage = dto.Dosage;
            medication.Frequency = dto.Frequency;
            medication.PrescribedDate = dto.PrescribedDate;

            await _context.SaveChangesAsync();
            return Ok("Medication updated.");
        }

        [Authorize(Roles = "Doctor")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedication(int id)
        {
            var medication = await _context.Medications.FindAsync(id);
            if (medication == null) return NotFound();

            // Also delete related links from PatientMedications
            var links = _context.PatientMedications.Where(pm => pm.MedicationID == id);
            _context.PatientMedications.RemoveRange(links);

            _context.Medications.Remove(medication);
            await _context.SaveChangesAsync();
            return Ok("Medication deleted.");
        }
    }

}

