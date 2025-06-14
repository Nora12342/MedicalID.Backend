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
                    pc.Condition.ConditionName  
                })
                .ToListAsync();

            return Ok(conditions);
        }

        
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
