using MedicalID.Backend.Data;
using MedicalID.Backend.Dtos;
using MedicalID.Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicalID.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicationsController : ControllerBase
    {
        private readonly MedicalIDContext _context;

        public MedicationsController(MedicalIDContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicationDto>>> GetMedications()
        {
            var medications = await _context.Medications
                .Select(m => new MedicationDto
                {
                    MedicationID = m.MedicationID,
                    MedicationName = m.MedicationName,
                    Dosage = m.Dosage,
                    Frequency = m.Frequency,
                    PrescribedDate = (DateTime)m.PrescribedDate,
                    PatientID = m.PatientID
                }).ToListAsync();

            return Ok(medications);
        }

        [HttpPost]
        public async Task<ActionResult<Medication>> PostMedication(MedicationDto dto)
        {
            var medication = new Medication
            {
                MedicationName = dto.MedicationName,
                Dosage = dto.Dosage,
                Frequency = dto.Frequency,
                PrescribedDate = dto.PrescribedDate,
                PatientID = dto.PatientID
            };

            _context.Medications.Add(medication);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMedications), new { id = medication.MedicationID }, medication);
        }
    }

}
