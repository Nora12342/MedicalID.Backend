using MedicalID.Backend.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicalID.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        
        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var patientsCount = await _context.Patients.CountAsync();
            var doctorsCount = await _context.Doctors.CountAsync();
            var appointmentsToday = await _context.Appointments
                .CountAsync(a => a.AppointmentDate.Date == DateTime.UtcNow.Date);

            var pendingDoctors = await _context.Doctors
                .CountAsync(d => d.ApprovalStatus == "Pending");

            return Ok(new
            {
                patients = patientsCount,
                doctors = doctorsCount,
                appointmentsToday = appointmentsToday,
                pendingApprovals = pendingDoctors
            });
        }

      
        [HttpGet("revenues")]
        public async Task<IActionResult> GetRevenues()
        {
            var totalRevenue = await _context.AskDoctors
                .Where(q => q.IsPaid)
                .SumAsync(q => q.AmountPaid);

            return Ok(new { revenue = totalRevenue });
        }

       
        [HttpGet("recent-doctors")]
        public async Task<IActionResult> GetRecentDoctors()
        {
            var recentDoctors = await _context.Doctors
                .OrderByDescending(d => d.DoctorID)
                .Take(3)
                .Select(d => new
                {
                    name = "Dr. " + d.FName + " " + d.LName,
                    specialization = d.Specialization.Name,
                    status = d.ApprovalStatus
                })
                .ToListAsync();

            return Ok(recentDoctors);
        }
    }
}

