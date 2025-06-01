using MedicalID.Backend.Data;
using MedicalID.Backend.Dtos;
using MedicalID.Backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MedicalID.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        private readonly IPasswordHasher<Doctor> _doctorHasher;
        private readonly IPasswordHasher<Patient> _patientHasher;

        public AuthController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
            _doctorHasher = new PasswordHasher<Doctor>();
            _patientHasher = new PasswordHasher<Patient>();
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDto dto)
        {
            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.UserName == dto.UserName);
            if (doctor != null)
            {
                if (BCrypt.Net.BCrypt.Verify(dto.Password, doctor.PasswordHash))
                {
                    var token = JwtHelper.GenerateToken(doctor.DoctorID, doctor.UserName, "Doctor", _config);
                    return Ok(new
                    {
                        token,
                        user = new LoginResponseDto
                        {
                            UserId = doctor.DoctorID,
                            Role = "Doctor",
                            UserName = doctor.UserName,
                            FullName = $"{doctor.FName} {doctor.LName}"
                        }
                    });
                }
                return Unauthorized("Invalid password.");
            }

            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.UserName == dto.UserName);
            if (patient != null)
            {
                if (BCrypt.Net.BCrypt.Verify(dto.Password, patient.PasswordHash))
                {
                    var token = JwtHelper.GenerateToken(patient.PatientID, patient.UserName, "Patient", _config);
                    return Ok(new
                    {
                        token,
                        user = new LoginResponseDto
                        {
                            UserId = patient.PatientID,
                            Role = "Patient",
                            UserName = patient.UserName,
                            FullName = $"{patient.FName} {patient.LName}"
                        }
                    });
                }
                return Unauthorized("Invalid password.");
            }

            return NotFound("User not found.");
        }

        [HttpPost("register/doctor")]
        public async Task<ActionResult> RegisterDoctor(DoctorRegisterDto dto)
        {
            if (await _context.Doctors.AnyAsync(d => d.UserName == dto.UserName))
                return BadRequest("Username already exists for a doctor.");

            var specialization = await _context.Specializations
                .FirstOrDefaultAsync(s => s.Name == dto.Specialization);

            if (specialization == null)
                return BadRequest("Specialization not found.");

            var doctor = new Doctor
            {
                DoctorID = dto.DoctorID,
                FName = dto.FName,
                LName = dto.LName,
                SpecializationID = specialization.SpecializationID,
                UserName = dto.UserName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                RegionID = dto.RegionID,
                Email = dto.Email,
                Phone = dto.Phone,
                ReferenceID = dto.ReferenceID
            };

            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            return Ok("Doctor registered successfully.");
        }


        [HttpPost("register/patient")]
        public async Task<ActionResult> RegisterPatient(PatientRegisterDto dto)
        {
            if (await _context.Patients.AnyAsync(p => p.UserName == dto.UserName))
                return BadRequest("Username already exists for a patient.");

            var patient = new Patient
            {
                PatientID = dto.PatientID,
                MedicalID = dto.MedicalID,
                FName = dto.FName,
                LName = dto.LName,
                UserName = dto.UserName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Gender = dto.Gender,
                DateOfBirth = dto.DateOfBirth,
                Email = dto.Email,
                BloodType = dto.BloodType,
                EmergencyContact = dto.EmergencyContact,
                OrganDonorStatus = dto.OrganDonorStatus,
                RegionID = dto.RegionID
            };

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            return Ok("Patient registered successfully.");
        }


        [HttpPost("Doctor/ForgotPassword")]
        public IActionResult DoctorForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            var doctor = _context.Doctors.FirstOrDefault(d => d.Email == dto.Email);
            if (doctor == null) return NotFound("Doctor with this email does not exist.");

            doctor.ResetToken = Guid.NewGuid().ToString();
            doctor.ResetTokenExpiry = DateTime.UtcNow.AddMinutes(15);
            _context.SaveChanges();

            return Ok(new { token = doctor.ResetToken });
        }

        [HttpPost("Doctor/ResetPassword")]
        public IActionResult DoctorResetPassword([FromBody] ResetPasswordDto dto)
        {
            var doctor = _context.Doctors.FirstOrDefault(d => d.ResetToken == dto.Token && d.ResetTokenExpiry > DateTime.UtcNow);
            if (doctor == null) return BadRequest("Invalid or expired token.");

            doctor.PasswordHash = _doctorHasher.HashPassword(doctor, dto.NewPassword);
            doctor.ResetToken = null;
            doctor.ResetTokenExpiry = null;
            _context.SaveChanges();

            return Ok("Password reset successful.");
        }


        [HttpPost("Patient/ForgotPassword")]
        public IActionResult PatientForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            var patient = _context.Patients.FirstOrDefault(p => p.Email == dto.Email);
            if (patient == null) return NotFound("Patient with this email does not exist.");

            patient.ResetToken = Guid.NewGuid().ToString();
            patient.ResetTokenExpiry = DateTime.UtcNow.AddMinutes(15);
            _context.SaveChanges();

            
            return Ok(new { token = patient.ResetToken });
        }

        [HttpPost("Patient/ResetPassword")]
        public IActionResult PatientResetPassword([FromBody] ResetPasswordDto dto)
        {
            var patient = _context.Patients.FirstOrDefault(p => p.ResetToken == dto.Token && p.ResetTokenExpiry > DateTime.UtcNow);
            if (patient == null) return BadRequest("Invalid or expired token.");

            patient.PasswordHash = _patientHasher.HashPassword(patient, dto.NewPassword);
            patient.ResetToken = null;
            patient.ResetTokenExpiry = null;
            _context.SaveChanges();

            return Ok("Password reset successful.");
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return Ok(new { message = "Logout successful" });
        }
    }
}

