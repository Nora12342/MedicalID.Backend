using MedicalID.Backend.Data;
using MedicalID.Backend.Dtos.Appointment;
using MedicalID.Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MedicalIDSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AppointmentsController(AppDbContext context)
        {
            _context = context;
        }

        // 🧑‍🦰 PATIENT: Book appointment
        [Authorize(Roles = "Patient")]
        [HttpPost]
        public async Task<IActionResult> BookAppointment([FromBody] AppointmentDTO dto)
        {
            var patientIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(patientIdClaim, out int patientId))
                return Unauthorized("Invalid patient ID in token.");

            if (patientId != dto.PatientID)
                return Forbid("You cannot book an appointment for another patient.");

            var appointment = new Appointment
            {
                PatientID = dto.PatientID,
                DoctorID = dto.DoctorID,
                AppointmentDate = dto.AppointmentDate,
                AppointmentType = dto.AppointmentType,
                Status = "Scheduled",
                Notes = dto.Notes
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return Ok("Appointment booked.");
        }

        // 👨‍⚕️ DOCTOR: View their appointments
        [Authorize(Roles = "Doctor")]
        [HttpGet("doctor")]
        public async Task<ActionResult<IEnumerable<AppointmentDTO>>> GetDoctorAppointments()
        {
            var doctorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var appointments = await _context.Appointments
                .Where(a => a.DoctorID == doctorId)
                .Include(a => a.Patient) // Assuming you have a navigation property named 'Patient'
                .ToListAsync();

            return Ok(appointments.Select(a => new AppointmentDTO
            {
                AppointmentID = a.AppointmentID,
                PatientID = a.PatientID,
                DoctorID = a.DoctorID,
                PatientName = $"{a.Patient.FName} {a.Patient.LName}", // Assuming Patient has FirstName and LastName
                AppointmentDate = a.AppointmentDate,
                AppointmentType = a.AppointmentType,
                Status = a.Status,
                Notes = a.Notes
            }));
        }

        [Authorize(Roles = "Patient")]
        [HttpGet("patient")]
        public async Task<ActionResult<IEnumerable<AppointmentDTO>>> GetPatientAppointments()
        {
            var patientIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(patientIdClaim, out int patientId))
                return Unauthorized("Invalid patient ID in token.");

            var appointments = await _context.Appointments
                .Where(a => a.PatientID == patientId)
                .Include(a => a.Doctor) // Assuming you have a navigation property named 'Doctor'
                .ToListAsync();

            return Ok(appointments.Select(a => new AppointmentDTO
            {
                AppointmentID = a.AppointmentID,
                PatientID = a.PatientID,
                DoctorID = a.DoctorID,
                DoctorName = $"{a.Doctor.FName} {a.Doctor.LName}", // Assuming Doctor has FirstName and LastName
                AppointmentDate = a.AppointmentDate,
                AppointmentType = a.AppointmentType,
                Status = a.Status,
                Notes = a.Notes
            }));
        }

        // 🛠️ DOCTOR: Update appointment status
        [Authorize(Roles = "Doctor")]
        [HttpPut("status/{id}")]
        public async Task<IActionResult> UpdateAppointmentStatus(int id, [FromBody] string newStatus)
        {
            var doctorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
                return NotFound("Appointment not found.");

            if (appointment.DoctorID != doctorId)
                return Forbid("You are not authorized to modify this appointment.");

            appointment.Status = newStatus;
            await _context.SaveChangesAsync();

            return Ok("Appointment status updated.");
        }
    }
}


