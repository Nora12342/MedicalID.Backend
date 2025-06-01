using MedicalID.Backend.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicalID.Backend.Controllers
{
    public class PatientDataController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PatientDataController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ 1. GET /api/PatientData/Conditions/{patientId}
        [HttpGet("Conditions/{patientId}")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetPatientConditions(int patientId)
        {
            var conditions = await _context.PatientConditions
                .Where(pc => pc.PatientID == patientId)
                .Include(pc => pc.Condition)
                .Select(pc => new
                {
                    pc.ConditionID,
                    pc.Condition.ConditionName,
                    pc.Note
                })
                .ToListAsync();

            return Ok(conditions);
        }

        // ✅ 2. GET /api/PatientData/Allergies/{patientId}
        [HttpGet("Allergies/{patientId}")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetPatientAllergies(int patientId)
        {
            var allergies = await _context.PatientAllergies
                .Where(pa => pa.PatientID == patientId)
                .Include(pa => pa.Allergy)
                .Select(pa => new
                {
                    pa.AllergyID,
                    pa.Allergy.Allergen,
                    pa.Allergy.Severity,
                    pa.Allergy.Reaction
                })
                .ToListAsync();

            return Ok(allergies);
        }

        // ✅ 3. GET /api/PatientData/Medications/{patientId}
        [HttpGet("Medications/{patientId}")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetPatientMedications(int patientId)
        {
            var medications = await _context.PatientMedications
                .Where(pm => pm.PatientID == patientId)
               .Select(pm => new
               {
                   pm.Medication.MedicationName,
                   pm.Medication.Dosage,
                   pm.Medication.Frequency,
                   pm.Medication.PrescribedDate
               })

                .ToListAsync();

            return Ok(medications);
        }
    }
}
