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
            var patientIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(patientIdStr))
                return Unauthorized("Authentication failed: Patient ID missing in token.");

            if (!int.TryParse(patientIdStr, out int patientIdAsInt))
            {
                return Unauthorized("Authentication failed: Invalid patient ID format in token.");
            }

            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.ID == patientIdAsInt);
            if (patient == null)
                return Unauthorized("Patient not found.");

            // Get medications through the PatientMedication join table
            var result = await _context.PatientMedications
                .Where(pm => pm.PatientID == patient.ID)
                .Include(pm => pm.Medication) 
                .Select(pm => new MedicationDto
                {
                    MedicationID = pm.MedicationID,
                    MedicationName = pm.Medication.MedicationName, 
                    Dosage = pm.Medication.Dosage,                 
                    Frequency = pm.Medication.Frequency,          
                    PrescribedDate = pm.Medication.PrescribedDate, 
                    
                })
                .ToListAsync();

            return Ok(result);
        }



        [HttpPost]
        public async Task<IActionResult> AddMedication([FromBody] MedicationPostDto dto)
        {
            var patientIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(patientIdStr))
                return Unauthorized("Authentication failed: Patient ID missing in token.");

            if (!int.TryParse(patientIdStr, out int patientIdAsInt))
            {
                return Unauthorized("Authentication failed: Invalid patient ID format in token.");
            }

            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.ID == patientIdAsInt);
            if (patient == null)
                return Unauthorized("Patient not found.");

            
            var existingMedication = await _context.Medications.FirstOrDefaultAsync(m =>
                m.MedicationName == dto.MedicationName &&
                m.Dosage == dto.Dosage &&
                m.Frequency == dto.Frequency);

            Medication medicationToLink;

            if (existingMedication == null)
            {
               
                medicationToLink = new Medication
                {
                    MedicationName = dto.MedicationName,
                    Dosage = dto.Dosage,
                    Frequency = dto.Frequency,
                    PrescribedDate = dto.PrescribedDate,
                    
                };
                _context.Medications.Add(medicationToLink);
                await _context.SaveChangesAsync(); 
            }
            else
            {
                medicationToLink = existingMedication;
            }

            
            var patientMedicationExists = await _context.PatientMedications
                .AnyAsync(pm => pm.PatientID == patient.ID && pm.MedicationID == medicationToLink.MedicationID);

            if (patientMedicationExists)
            {
                return BadRequest("This medication is already linked to this patient.");
            }

            
            var patientMedication = new PatientMedication
            {
                PatientID = patient.ID,
                MedicationID = medicationToLink.MedicationID,
                
            };

            _context.PatientMedications.Add(patientMedication);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMyMedications), new { id = medicationToLink.MedicationID }, medicationToLink);
        }

        [HttpDelete("{medicationId}")]
        public async Task<IActionResult> DeleteMedication(int medicationId)
        {
            var patientIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(patientIdStr))
                return Unauthorized("Authentication failed: Patient ID missing in token.");

            if (!int.TryParse(patientIdStr, out int patientIdAsInt))
            {
                return Unauthorized("Authentication failed: Invalid patient ID format in token.");
            }

            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.ID == patientIdAsInt);
            if (patient == null)
                return Unauthorized("Patient not found.");

            
            var patientMedicationRecord = await _context.PatientMedications
                .FirstOrDefaultAsync(pm => pm.PatientID == patient.ID && pm.MedicationID == medicationId);

            if (patientMedicationRecord == null)
            {
                return NotFound("Medication not found for this patient.");
            }

            _context.PatientMedications.Remove(patientMedicationRecord);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

